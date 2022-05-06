using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using SolaraNet.BusinessLogic.Abstracts;
using SolaraNet.BusinessLogic.Contracts;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.BusinessLogic.Implementations.Validators;
using SolaraNet.DataAccessLayer.Abstracts;
using SolaraNet.DataAccessLayer.Entities;

using SolaraNet.DataAccessLayer.EntityFramework.Common.Validators;
using SolaraNet.BusinessLogic.Implementations;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Mvc;


namespace SolaraNet.BusinessLogic.Implementations
{
    public partial class AdvertismentService : IAdvertismentService
    {
        #region Поля
        private readonly IAdvertismentRepository _advertismentRepository;
        private readonly ISaver _saver;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IUserRepository _userRepository;
        #endregion

        public AdvertismentService(IAdvertismentRepository advertismentRepository, ISaver saver, IMapper mapper, IIdentityService identityService, IUserRepository userRepository)
        {
            _advertismentRepository = advertismentRepository;
            _saver = saver;
            _mapper = mapper;
            _identityService = identityService;
            _userRepository = userRepository;
        }

        public async Task<OperationResult<IList<AdvertismentDTO>>> GetPaged(PageViewModel request,
            CancellationToken cancellationToken)
        {
            var userId = await _identityService.GetCurrentUserId(cancellationToken);
            Expression<Func<DBAdvertisment, bool>> originalExpressions = x => x.Status == request.RequeredStatus
                                                                              && x.Price <= request.MaximumCost && x.Price >= request.MinimumCost
                                                                              && ((request.Category != null) ? x.DBCategoryId == request.Category.Id : true)
                                                                              && ((request.City != null) ? x.City == request.City : true)
                                                                              && ((request.KeyWords != null)
                                                                                  ? (x.AdvertismentTitle.Contains(request.KeyWords) ||
                                                                                      x.Description.Contains(request.KeyWords))
                                                                                  : true)
                                                                              && ((request.Date != null) ? x.PublicationDate == request.Date : true)
                                                                              && ((request.OnlyWithComments) ? x.Comments.Count >= 1 : true)
                                                                              && ((request.OnlyWithPhoto) ? x.Image.Count >= 1 : true);

            List<DBAdvertisment> advertisments =
                await _advertismentRepository.GetPaged(new PaginationModel()
                {
                    PredicateWhere = originalExpressions,
                    ByDescendingCost = request.ByDescending,
                    Limit = request.AdvertismentPerPage,
                    Offset = request.Offset,
                    PredicateOrderBy = (request.SortByCost?x=>x.Price:x=>x.PublicationDate)
                }, cancellationToken);
            if (request.Category!=null)
            {
                var categories = await
                    _advertismentRepository.GetUndercategoryOfCategoriesByCategoryId(request.Category.Id,
                        cancellationToken);
                await GetAdvertismentsOfUnderCategory((List<DBCategory>) categories);

                async Task GetAdvertismentsOfUnderCategory(List<DBCategory> categories)
                {
                    foreach (var category in categories)
                    {
                        await AddUnderCategoriesAdvertisments(category);
                        if (category.UnderCategories!=null)
                        {
                            if (category.UnderCategories.Count>0)
                            {
                                await GetAdvertismentsOfUnderCategory(category.UnderCategories);
                            }
                        }
                    }
                }
                async Task<bool> AddUnderCategoriesAdvertisments(DBCategory category)
                {
                    var additionalAdvertisments = await _advertismentRepository.GetPaged(
                        new PaginationModel()
                        {
                            PredicateWhere = x => x.Status == request.RequeredStatus
                                                  && x.Price < request.MaximumCost && x.Price > request.MinimumCost
                                                  && ((category.Id != null) ? x.DBCategoryId == category.Id : true)
                                                  && ((request.City != null) ? x.City == request.City : true)
                                                  && ((request.KeyWords != null)
                                                      ? x.AdvertismentTitle.Contains(request.KeyWords) ||
                                                        x.Description.Contains(request.KeyWords)
                                                      : true)
                                                  && ((request.Date != null) ? x.PublicationDate == request.Date : true)
                                                  && ((request.OnlyWithComments) ? x.Comments.Count >= 1 : true)
                                                  && ((request.OnlyWithPhoto) ? x.Image.Count >= 1 : true),
                            ByDescendingCost = request.ByDescending,
                            Limit = request.AdvertismentPerPage,
                            Offset = request.Offset,
                            PredicateOrderBy = (request.SortByCost ? x => x.Price : x => x.PublicationDate)
                        }, cancellationToken);
                    advertisments.AddRange(additionalAdvertisments);
                    return true;
                }
            }

            if (request.SortByCost)
            {
                advertisments = (request.ByDescending)
                    ? advertisments.OrderByDescending(x=>x.Price).ToList()
                    : advertisments.OrderBy(x=>x.Price).ToList();
            }
            else
            {
                advertisments = (request.ByDescending)
                    ? advertisments.OrderByDescending(x => x.PublicationDate).ToList()
                    : advertisments.OrderBy(x => x.PublicationDate).ToList();
            }

            List<AdvertismentDTO> advertismentsToReturn = new List<AdvertismentDTO>();
            foreach (var value in advertisments)
            {
                var advertismet = _mapper.Map<AdvertismentDTO>(value);
                var userName = await _identityService.GetUserById(advertismet.UserId, cancellationToken);
                advertismet.UserName = userName.Result.Name;
                advertismet.ImagesList = new List<string>();
                foreach (var image in value.Image)
                {
                    advertismet.ImagesList.Add(image.ImageLink);
                }
                advertismet.CommentCount = value.Comments.Count(x => x.CommentStatus==EntityStatus.Active);
                advertismet.UserPhone = value.User?.PhoneNumber;
                advertismentsToReturn.Add(advertismet);
            }

            return OperationResult<IList<AdvertismentDTO>>.Ok(advertismentsToReturn);

        }

        public async Task<OperationResult<IList<AdvertismentDTO>>> GetPagedMyAdvertisments(MyPaginationModel request, CancellationToken cancellationToken)
        {
            var userId = await _identityService.GetCurrentUserId(cancellationToken);
            var paginationModel = _mapper.Map<PaginationModel>(request);
            paginationModel.PredicateWhere = x => x.UserId == userId.Result && ((request.HideDeleted) ? x.Status == EntityStatus.Active || x.Status == EntityStatus.Created || x.Status == EntityStatus.Rejected : true);
            paginationModel.PredicateOrderBy = x => x.PublicationDate;
            var result = await _advertismentRepository.GetPaged(paginationModel, cancellationToken);
            List<AdvertismentDTO> advertismentsToReturn = new List<AdvertismentDTO>();
            foreach (var value in result)
            {
                var advertismet = _mapper.Map<AdvertismentDTO>(value);
                var userName = await _identityService.GetUserById(advertismet.UserId, cancellationToken);
                advertismet.UserName = userName.Result.Name;
                advertismet.ImagesList = new List<string>();
                advertismet.CommentCount = value.Comments.Count(x => x.CommentStatus == EntityStatus.Active);
                foreach (var image in value.Image)
                {
                    advertismet.ImagesList.Add(image.ImageLink);
                }

                advertismentsToReturn.Add(advertismet);
            }

            return OperationResult<IList<AdvertismentDTO>>.Ok(advertismentsToReturn);
        }

        public async Task<OperationResult<IList<AdvertismentDTO>>> GetPagedAdvertismentsByUserId(
            PaginationByUserId request, CancellationToken cancellationToken)
        {
            var paginationModel = _mapper.Map<PaginationModel>(request);
            paginationModel.PredicateWhere = x => x.Status == EntityStatus.Active && x.UserId == request.UsedId;
            paginationModel.PredicateOrderBy = x => x.PublicationDate;
            var result = await _advertismentRepository.GetPaged(paginationModel, cancellationToken);
            List<AdvertismentDTO> advertismentsToReturn = new List<AdvertismentDTO>();
            foreach (var value in result)
            {
                var advertismet = _mapper.Map<AdvertismentDTO>(value);
                var userName = await _identityService.GetUserById(advertismet.UserId, cancellationToken);
                advertismet.UserName = userName.Result.Name;
                advertismet.ImagesList = new List<string>();
                advertismet.CommentCount = value.Comments.Count(x => x.CommentStatus == EntityStatus.Active);
                foreach (var image in value.Image)
                {
                    advertismet.ImagesList.Add(image.ImageLink);
                }

                advertismentsToReturn.Add(advertismet);
            }

            return OperationResult<IList<AdvertismentDTO>>.Ok(advertismentsToReturn);
        }

        public async Task<OperationResult<int>> GetUserIdAdvertismentsPagesCount(PaginationByUserId request,
            CancellationToken cancellationToken)
        {
            Expression<Func<DBAdvertisment, bool>> whereExpression =
                x => x.Status == EntityStatus.Active && x.UserId == request.UsedId;
            var totalCountOfAdvertisments = await _advertismentRepository.AdvertismentsCount(whereExpression, cancellationToken);

            return OperationResult<int>.Ok(GetPageCount(totalCountOfAdvertisments, request.Limit));
        }

        public async Task<OperationResult<bool>> CreateNewAdvertisment(AdvertismentDTO advertismentToBeAdded, CancellationToken cancellationToken)
        {
            var userId = await _identityService.GetCurrentUserId(cancellationToken);
            advertismentToBeAdded.UserId = userId.Result;
            advertismentToBeAdded.AdvertismentID = default; // сейчас нам это не важно
            
            try
            {
                await ValidateAdvertisment(advertismentToBeAdded, false, cancellationToken); // тут валидировать Id смысла нет
            }
            catch (Exception e)
            {
                return OperationResult<bool>.Failed(new[] {"Не удалось свалидировать AdvertismentDTO. Подробности: " + e.Message });
            }

            var dbAdvertisment = _mapper.Map<DBAdvertisment>(advertismentToBeAdded);
            var user = await _userRepository.GetUserWhere(x => x.Id == userId.Result, cancellationToken);
            dbAdvertisment.User = user;  // привязываем это объявление к этому юзеру
            dbAdvertisment.DBCategory = new DBCategory();
            dbAdvertisment.DBCategory =
                await _advertismentRepository.GetCategoryById(advertismentToBeAdded.Category.Id, cancellationToken);
            dbAdvertisment.Image = new List<DBImageOfAdvertisment>();
            dbAdvertisment.Status = EntityStatus.Created;
            foreach (var link in advertismentToBeAdded.ImagesList)
            {
                dbAdvertisment.Image.Add(new DBImageOfAdvertisment(){ImageLink = link, Status = EntityStatus.Active});
            }
            await _advertismentRepository.CreateNewAdvertisment(dbAdvertisment, cancellationToken);
            await _saver.SaveAllChanges();

            return OperationResult<bool>.Ok(true);
        }

        public async Task<OperationResult<AdvertismentDTO>> GetById(int id, CancellationToken cancellationToken)
        {
            var validResult = await IsIntValueValid(id, cancellationToken);
            if (!validResult)
            {
                return OperationResult<AdvertismentDTO>.Failed(new []{"В метод GetById был передан невалидный id"});
            }

            var dbAdvertisment = await _advertismentRepository.GetAdvertismentById(id, cancellationToken);
            var advertismentDto = _mapper.Map<AdvertismentDTO>(dbAdvertisment);
            var userName = await _identityService.GetUserById(advertismentDto.UserId, cancellationToken);
            advertismentDto.UserName = userName.Result.Name;
            advertismentDto.UserPhone = dbAdvertisment.User?.PhoneNumber;
            advertismentDto.ImagesList = new List<string>();
            advertismentDto.CommentCount = dbAdvertisment.Comments.Count(x => x.CommentStatus == EntityStatus.Active);
            if (dbAdvertisment.Image!=null)
            {
                foreach (var image in dbAdvertisment.Image)
                {
                    advertismentDto.ImagesList.Add(image.ImageLink);
                }
            }
            return OperationResult<AdvertismentDTO>.Ok(advertismentDto);
        }

        public async Task<OperationResult<bool>> Delete(int id, CancellationToken cancellationToken)
        {
            var currentUserId = await _identityService.GetCurrentUserId(cancellationToken);
            var thisAdvertisment =
                await _advertismentRepository.GetAdvertismentById(id,
                    cancellationToken);
            if (thisAdvertisment == null)
            {
                return OperationResult<bool>.Failed(new[] { "Объявления с таким Id попросту нет!" });
            }
            var isUserAdmin = await _identityService.IsInRole(currentUserId.Result, "Admin", cancellationToken); // true - текущий пользователь является администратором, false - не явялется
            if ((currentUserId.Result != thisAdvertisment.UserId) && !isUserAdmin.Result)
            {
                return OperationResult<bool>.Failed(new[] { "У текущего пользователя нет прав для удаления данного объявления! Только администратор и автор комментария может это сделать!" });
            }
            var validResult = await IsIntValueValid(id, cancellationToken);
            if (!validResult)
            {
                return OperationResult<bool>.Failed(new []{"В метод Delete был передан невалидный id"});
            }

            if (isUserAdmin.Result)
            {
                await _advertismentRepository.DeleteAdvertisment(id, true, cancellationToken);
                await _saver.SaveAllChanges();
                return OperationResult<bool>.Ok(true);
            }
            await _advertismentRepository.DeleteAdvertisment(id, false, cancellationToken);
            await _saver.SaveAllChanges();
            return OperationResult<bool>.Ok(true);
        }

        public async Task<OperationResult<bool>> DeleteAdvertismentsByUserId(string userId,
            CancellationToken cancellationToken)
        {
            var advertisments = await _advertismentRepository.GetPaged(new PaginationModel()
            {
                PredicateWhere = x=>x.UserId==userId,
                ByDescendingCost = false,
                Limit = 99999,
                Offset = 0,
                PredicateOrderBy = x=>x.PublicationDate
            }, cancellationToken);
            if (advertisments!=null)
            {
                foreach (var advertisment in advertisments)
                {
                    await Delete(advertisment.Id, cancellationToken);
                }
            }
            return OperationResult<bool>.Ok(true);
        }

        public async Task<OperationResult<bool>> Update(AdvertismentDTO updatedAdvertismentDto, CancellationToken cancellationToken)
        {
            try
            {
                await ValidateAdvertisment(updatedAdvertismentDto, true, cancellationToken);
            }
            catch (System.Exception e)
            {
                return OperationResult<bool>.Failed(new[] { e.Message });
            }
            var userId = await _identityService.GetCurrentUserId(cancellationToken);
            var thisAdvertisment =
                await _advertismentRepository.GetAdvertismentById(updatedAdvertismentDto.AdvertismentID,
                    cancellationToken);
            if (thisAdvertisment == null)
            {
                return OperationResult<bool>.Failed(new[] { "Объявления с таким Id попросту нет!" });
            }
            var isUserAdmin = await _identityService.IsInRole(userId.Result, "Admin", cancellationToken); // true - текущий пользователь является администратором, false - не явялется
            if ((userId.Result != thisAdvertisment.UserId) && !isUserAdmin.Result)
            {
                return OperationResult<bool>.Failed(new[] { "У текущего пользователя нет прав для редактирования данного объявления! Только администратор и автор объявления может это сделать!" });
            }

            var newAdvertisment = _mapper.Map<DBAdvertisment>(updatedAdvertismentDto);
            newAdvertisment.Image = new List<DBImageOfAdvertisment>();
            foreach (var image in updatedAdvertismentDto.ImagesList)
            {
                newAdvertisment.Image.Add(new DBImageOfAdvertisment()
                {
                    ImageLink = image
                });
            }
            var result = await _advertismentRepository.UpdateAdvertisment(updatedAdvertismentDto.AdvertismentID,
                newAdvertisment, cancellationToken);
            if (!result)
            {
                return OperationResult<bool>.Failed(new []{"Не удалось обновить объявление по причине того, что его нет в принципе, обновлять нечего."});
            }

            await _saver.SaveAllChanges();
            return OperationResult<bool>.Ok(true);
        }

        public async Task<OperationResult<int>> GetPagesCount(PageCountModel request, CancellationToken cancellationToken)
        {
            Expression<Func<DBAdvertisment, bool>> originalExpressions = x => x.Status == request.Status
                                                                              && x.Price < request.MaximumCost && x.Price > request.MinimumCost
                                                                              && ((request.IdCategory != null) ? x.DBCategoryId == request.IdCategory : true)
                                                                              && ((request.City != null) ? x.City == request.City : true)
                                                                              && ((request.KeyWords != null)
                                                                                  ? x.AdvertismentTitle.Contains(request.KeyWords) ||
                                                                                  x.Description.Contains(request.KeyWords)
                                                                                  : true)
                                                                              && ((request.Date != null) ? x.PublicationDate == request.Date : true)
                                                                              && ((request.OnlyWithComments) ? x.Comments.Count >= 1 : true)
                                                                              && ((request.OnlyWithPhoto) ? x.Image.Count >= 1 : true);
           
            var totalCountOfAdvertisments = await 
                _advertismentRepository.AdvertismentsCount(
                    originalExpressions, cancellationToken);
            return OperationResult<int>.Ok(GetPageCount(totalCountOfAdvertisments, request.AdvertismentsPerPage));
        }

        public async Task<OperationResult<int>> GetMyPagesCount(MyPaginationModel model, CancellationToken cancellationToken)
        {
            var userId = await _identityService.GetCurrentUserId(cancellationToken);
            Expression<Func<DBAdvertisment, bool>> expression = x =>
                x.User.Id == userId.Result && ((model.HideDeleted) ? x.Status == EntityStatus.Active : true);
            var totalCount = await _advertismentRepository.AdvertismentsCount(expression, cancellationToken);
            var count = GetPageCount(totalCount, model.Limit);

            return OperationResult<int>.Ok(count);
        }


        /// <summary>
        /// Возвращает цену самое дорогое объявление
        /// </summary>
        /// <returns></returns>
        public async Task<OperationResult<decimal>> GetTheMostExpensiveAdvertismentCost(PageViewModel request, CancellationToken cancellationToken)
        {
            decimal ifNoAdvertismentValue = 0; // если нет объявлений, возвращаем это значение
            var advertisments = await GetPaged(request, cancellationToken); // получили объявления с учётом заданных пользователем сортировок
            if (!advertisments.Success)
            {
                return OperationResult<decimal>.Failed(new []{advertisments.GetErrors()}); // возвращаем спиоск ошибок
            }
            if (advertisments.Result==null)
            {
                return OperationResult<decimal>.Ok(ifNoAdvertismentValue); 
            }
            if (advertisments.Result.Count<1)
            {
                return OperationResult<decimal>.Ok(ifNoAdvertismentValue);
            }

            return OperationResult<decimal>.Ok(advertisments.Result.Max(x => x.Price));
        }

        public async Task<OperationResult<bool>> ApproveAdvertisment(int id, CancellationToken cancellationToken)
        {
            var advertisment = await _advertismentRepository.GetAdvertismentById(id, cancellationToken);
            advertisment.Status = EntityStatus.Active;
            await _saver.SaveAllChanges();
            return OperationResult<bool>.Ok(true);
        }

        public async Task<OperationResult<bool>> RejectAdvertisment(RejectionModel model, CancellationToken cancellationToken)
        {
            var advertisment = await _advertismentRepository.GetAdvertismentById(model.AdvertismentId, cancellationToken);
            advertisment.Status = EntityStatus.Rejected;
            advertisment.DeleteReason = model.DeleteReason;
            await _saver.SaveAllChanges();
            return OperationResult<bool>.Ok(true);
        }

        private async Task<bool> IsIntValueValid(int value, CancellationToken cancellationToken)
        {
            IntValidator newIntValidator = new IntValidator();
            var result = await newIntValidator.ValidateAsync(value, cancellationToken);
            if (!result.IsValid)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Валидация объявления. Если булевское значение validateId true, то валидируем id, иначе - нет.
        /// </summary>
        /// <param name="advertisment"></param>
        /// <param name="validateId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task ValidateAdvertisment(AdvertismentDTO advertisment, bool validateId, CancellationToken cancellationToken)
        {
            var stringResultTitile = await Task.Run((() => StringValidator.CheckString(advertisment.AdvertismentTitle)), cancellationToken);
            var stringResultDescription = await Task.Run((() => StringValidator.CheckString(advertisment.Description)), cancellationToken);
            if (validateId)
            {
                AdvertismentDTOValidator validator = new AdvertismentDTOValidator();
                var result = await validator.ValidateAsync(advertisment, cancellationToken);
                if (!result.IsValid && !stringResultTitile && !stringResultDescription)
                {
                    throw new ArgumentException("Ошибка валидации объявления");
                }
            }
            else
            { 
                AdvertismentDTOValidatorFromApi validator = new AdvertismentDTOValidatorFromApi();
                var result = await validator.ValidateAsync(advertisment, cancellationToken);
                if (!result.IsValid && !stringResultTitile && !stringResultDescription)
                {
                    throw new ArgumentException("Ошибка валидации объявления");
                }
            }
        }

        /// <summary>
        /// Валидация категории. Если булевское значение validateId true, то валидируем id, иначе - нет.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="validateId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task ValidateCategory(CategoryDTO category, bool validateId, CancellationToken cancellationToken)
        {
            if (validateId)
            {
                CategoryDTOValidator validator = new CategoryDTOValidator();
                var result = await validator.ValidateAsync(category, cancellationToken);
                if (!result.IsValid)
                {
                    throw new ArgumentException("Ошибка валидации категории");
                }
            }
            else
            {
                CategoryDTOValidatorFromApi validator = new CategoryDTOValidatorFromApi();
                var result = await validator.ValidateAsync(category, cancellationToken);
                if (!result.IsValid)
                {
                    throw new ArgumentException("Ошибка валидации категории");
                }
            }
        }
        private int GetPageCount(int advertismentsCount, int perPage)
        {
            float result = (float)advertismentsCount / perPage;
            var truncatedValue = Math.Truncate(result); // округлили число
            var faction = result - truncatedValue; // получили дробную часть
            int finalResul = 0;
            if (faction>0)
            {
                finalResul = (int) result + 1;
            }
            else
            {
                finalResul = (int) result;
            }
            return finalResul;
        }
    }
}
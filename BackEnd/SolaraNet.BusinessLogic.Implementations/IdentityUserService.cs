using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SolaraNet.BusinessLogic.Abstracts;
using SolaraNet.BusinessLogic.Contracts;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.BusinessLogic.Implementations.Validators;
using SolaraNet.DataAccessLayer.Abstracts;
using SolaraNet.DataAccessLayer.Entities;
using SolaraNet.DataAccessLayer.EntityFramework.Common.Validators;

namespace SolaraNet.BusinessLogic.Implementations
{
    public class IdentityUserService : IIdentityUserService
    {
        #region Репозитории + identityService + всякое такое

        private readonly IUserRepository _userRepository;
        private readonly IIdentityService _identityService;
        private readonly IAdvertismentService _advertismentService;
        private readonly IMapper _mapper;
        private readonly ISaver _saver;
        #endregion

        public IdentityUserService(IUserRepository userRepository, IIdentityService identityService, IMapper mapper, ISaver saver, IAdvertismentService advertismentService)
        {
            _userRepository = userRepository;
            _identityService = identityService;
            _mapper = mapper;
            _saver = saver;
            _advertismentService = advertismentService;
        }

        public async Task<OperationResult<string>> Register(UserDTO user, CancellationToken cancellationToken)
        {
            UserDTOValidatorFromApi validator = new UserDTOValidatorFromApi();
            var result = await validator.ValidateAsync(user, cancellationToken);

            if (!result.IsValid)
            {
                return OperationResult<string>.Failed(new []{string.Join(';', result.Errors.Select(x=>x.ErrorMessage))});
            }

            var response = await _identityService.CreateUser(user, cancellationToken);

            if (!response.Success)
            {
                return OperationResult<string>.Failed(new []{string.Join(';',response.Errors)});
            }

            user.Id = response.Result;
            //var result1 = await _userRepository.CreateUser(_mapper.Map<DBUser>(user), cancellationToken);
            //if (!result1)
            //{
            //    return OperationResult<string>.Failed(new []{"Не удалось создать пользователя в базе данных, то месть метод _userRepository.CreateUser вернул false."});
            //}
            await _saver.SaveAllChanges(); // сохраняем изменения

            return OperationResult<string>.Ok(response.Result); // всё прошло успешно, возвращаем id user'a в формате GUID, а точнее в стринговском формате, но по сути GUID
        }

        public async Task<OperationResult<bool>> ChangePassword(ChangePasswordModel model, CancellationToken cancellationToken)
        {
            var result = await _identityService.ChangeUserPassword(model, cancellationToken);
            if (!result.Success)
            {
                return OperationResult<bool>.Failed(new []{result.GetErrors()});
            }
            return OperationResult<bool>.Ok(true);
        }

        public async Task<OperationResult<bool>> UpdateUserEmail(ChangeEmailModel model, CancellationToken cancellationToken)
        {
            var result = await _identityService.ChangeUserEmail(model, cancellationToken);
            if (!result.Success)
            {
                return OperationResult<bool>.Failed(new []{result.GetErrors()});
            }

            return OperationResult<bool>.Ok(true);
        }

        public async Task<OperationResult<bool>> ChangeUserMobilePhoneNumber(string number,
            CancellationToken cancellationToken)
        {
            var validateNumber = await Task.FromResult(StringValidator.CheckString(number));
            if (!validateNumber)
            {
                return OperationResult<bool>.Failed(new []{"Номер телефона невалиден"});
            }
            var result = await _identityService.ChangeMobilePhoneNumber(number, cancellationToken);
            if (!result.Success)
            {
                return OperationResult<bool>.Failed(new []{result.GetErrors()});
            }

            return OperationResult<bool>.Ok(true);
        }

        public async Task<OperationResult<bool>> ChangeUserName(string name, CancellationToken cancellationToken)
        {
            var validateName = await Task.FromResult(StringValidator.CheckString(name));
            if (!validateName)
            {
                return OperationResult<bool>.Failed(new []{"Имя пользователя невалидно"});
            }

            var result = await _identityService.ChangeUserName(name, cancellationToken);
            if (!result.Success)
            {
                return OperationResult<bool>.Failed(new[] { result.GetErrors() });
            }

            return OperationResult<bool>.Ok(true);
        }

        public async Task<OperationResult<bool>> BanUser(string id, CancellationToken cancellationToken)
        {
            var validateId = await Task.FromResult(StringValidator.CheckString(id));
            if (!validateId)
            {
                return OperationResult<bool>.Failed(new[] { "Id пользователя невалидно" });
            }
            var banResult = await _identityService.BanUser(id, cancellationToken);
            if (!banResult.Success)
            {
                return OperationResult<bool>.Failed(new []{banResult.GetErrors()});
            }
            // далее удаляем всё, что связано с этим пользователем
            var deleteUsersAdvertismentsResult =
                await _advertismentService.DeleteAdvertismentsByUserId(id, cancellationToken);
            if (!deleteUsersAdvertismentsResult.Success)
            {
                return OperationResult<bool>.Failed(new []{deleteUsersAdvertismentsResult.GetErrors()});
            }

            var deleteUsersCommentsResult = await _advertismentService.DeleteCommentsByUserId(id, cancellationToken);
            if (!deleteUsersCommentsResult.Success)
            {
                return OperationResult<bool>.Failed(new[] { deleteUsersCommentsResult.GetErrors() });
            }
            return OperationResult<bool>.Ok(true);

        }

    }
}
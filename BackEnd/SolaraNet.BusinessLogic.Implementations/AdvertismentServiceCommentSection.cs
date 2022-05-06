using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SolaraNet.BusinessLogic.Contracts;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.BusinessLogic.Implementations.Validators;
using SolaraNet.DataAccessLayer.Entities;
using SolaraNet.DataAccessLayer.EntityFramework.Common.Validators;

namespace SolaraNet.BusinessLogic.Implementations
{
    public partial class AdvertismentService
    {
        public async Task<OperationResult<bool>> AddComment(int advertismentId, CommentDTO comment, CancellationToken cancellationToken)
        {
            var userId = await _identityService.GetCurrentUserId(cancellationToken);
            await _advertismentRepository.AddComment(advertismentId, new DBComment()
            {
                UserId = userId.Result,
                AdvertismentId = advertismentId,
                CommentStatus = EntityStatus.Active,
                CommentText = comment.CommentText,
                PublicationDate = DateTime.Today
            }, cancellationToken);
            await _saver.SaveAllChanges();
            return OperationResult<bool>.Ok(true);
        }

        public async Task<OperationResult<IList<CommentDTO>>> GetPagedComments(CommentPageViewModel request, CancellationToken cancellationToken)
        {
            var validationResult = await IsCommentPageViewModelValid(request, cancellationToken);
            if (!validationResult)
            {
                return OperationResult<IList<CommentDTO>>.Failed(new []{ "CommentPageViewModel неправильная" });
            }
            var comments = await _advertismentRepository.GetPagedComments(x => x.AdvertismentId == request.AdvertismentId && x.CommentStatus==EntityStatus.Active,
                request.Offset, request.CommentsPerPage, cancellationToken);
            var dbComments = comments as DBComment[] ?? comments.ToArray();
            if (!IsListValid(dbComments))
            {
                return OperationResult<IList<CommentDTO>>.Failed(new []{"Комментариев в БД нет."});
            }
            List<CommentDTO> commentsToReturn = new List<CommentDTO>();
            foreach (var value in dbComments)
            {
                CommentDTO mappedComment = _mapper.Map<CommentDTO>(value);
                mappedComment.AvatarLink = value.User.Avatar?.ImageLink;
                commentsToReturn.Add(mappedComment);
            }

            return OperationResult<IList<CommentDTO>>.Ok(commentsToReturn);
        }
        
        public async Task<OperationResult<int>> GetPagesOfCommentsCount(CommentPageViewModel request, CancellationToken cancellationToken)
        {
            var totalCountOfAdvertisments = await
                _advertismentRepository.CommentsCount(
                    x => x.Advertisment.Id == request.AdvertismentId && x.CommentStatus==EntityStatus.Active, cancellationToken);
            if (! await IsIntValueValid(totalCountOfAdvertisments, cancellationToken))
            {
                return OperationResult<int>.Failed(new []{"Количество комментов стремится к минус бесконечности. Иначе говоря, комментариев нет"});
            }
            return OperationResult<int>.Ok(GetPageCount(totalCountOfAdvertisments, request.CommentsPerPage));
        }

        public async Task<OperationResult<bool>> DeleteComment(int commentId, CancellationToken cancellationToken)
        {
            var currentUserId = await _identityService.GetCurrentUserId(cancellationToken);
            var thisComment =
                await _advertismentRepository.GetCommentByWhere(x => x.Id == commentId,
                    cancellationToken);
            if (thisComment==null)
            {
                return OperationResult<bool>.Failed(new []{"Комментария с таким Id попросту нет!"});
            }
            var isUserAdmin = await _identityService.IsInRole(currentUserId.Result, "Admin", cancellationToken); // true - текущий пользователь является администратором, false - не явялется
            if ((currentUserId.Result != thisComment.UserId) && !isUserAdmin.Result)
            {
                return OperationResult<bool>.Failed(new []{"У текущего пользователя нет прав для удаления данного комментария! Только администратор и автор комментария может это сделать!"});
            }
            var validateResult = await _advertismentRepository.DeleteComment(commentId, cancellationToken);
            if (!validateResult)
            {
                return OperationResult<bool>.Failed(new []{"Не удалось удалить комментарий. Скорее всего, его попросту нет в базе данных."});
            }
            await _saver.SaveAllChanges();

            return OperationResult<bool>.Ok(true);
        }

        public async Task<OperationResult<bool>> DeleteCommentsByUserId(string userId,
            CancellationToken cancellationToken)
        {
            var comments = await 
                _advertismentRepository.GetPagedComments(x => x.UserId == userId, 0, 99999, cancellationToken);
            if (comments!=null)
            {
                foreach (var comment in comments)
                {
                    await _advertismentRepository.DeleteComment(comment.Id, cancellationToken);
                }
            }
            return OperationResult<bool>.Ok(true);
        }

        private async Task<bool> IsCommentValid(CommentDTO comment, CancellationToken cancellationToken)
        {
            CommentDTOValidator validator = new CommentDTOValidator();
            var validateComment = await validator.ValidateAsync(comment, cancellationToken);
            if (!validateComment.IsValid || !StringValidator.CheckString(comment.CommentText))
            {
                return false;
            }

            return true;
        }

        private async Task<bool> IsCommentPageViewModelValid(CommentPageViewModel model, CancellationToken cancellationToken)
        {
            IntValidator validator = new IntValidator();
            var validateIdResult = await validator.ValidateAsync(model.AdvertismentId, cancellationToken);
            if (!validateIdResult.IsValid)
            {
                return false; 
            }

            return true;
        }

        
    }
}
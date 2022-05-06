using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SolaraNet.BusinessLogic.Contracts;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.BusinessLogic.Implementations;

namespace SolaraNet.BusinessLogic.Abstracts
{
    public interface IAdvertismentService
    {
        Task<OperationResult<bool>> CreateNewAdvertisment(AdvertismentDTO advertismentToBeAdded, CancellationToken cancellationToken);
        Task<OperationResult<AdvertismentDTO>> GetById(int id, CancellationToken cancellationToken);
        Task<OperationResult<bool>> Delete(int id, CancellationToken cancellationToken);
        Task<OperationResult<bool>> Update(AdvertismentDTO model, CancellationToken cancellationToken);
        Task<OperationResult<bool>> AddComment(int advertismentId, CommentDTO comment, CancellationToken cancellationToken);
        Task<OperationResult<bool>> DeleteComment(int commentId, CancellationToken cancellationToken);
        Task<OperationResult<IList<AdvertismentDTO>>> GetPaged(PageViewModel request, CancellationToken cancellationToken);
        Task<OperationResult<int>> GetPagesCount(PageCountModel request, CancellationToken cancellationToken);
        Task<OperationResult<IList<CommentDTO>>> GetPagedComments(CommentPageViewModel request,
            CancellationToken cancellationToken);
        Task<OperationResult<int>> GetPagesOfCommentsCount(CommentPageViewModel request,
            CancellationToken cancellationToken);
        Task<OperationResult<IEnumerable<CategoryDTO>>> GetCategories(CancellationToken cancellationToken);
        Task<OperationResult<IEnumerable<CategoryDTO>>> GetUnderCategories(int parentCategoryId,
            CancellationToken cancellationToken);
        Task<OperationResult<IList<AdvertismentDTO>>> GetPagedMyAdvertisments(MyPaginationModel request,
            CancellationToken cancellationToken);
        Task<OperationResult<decimal>> GetTheMostExpensiveAdvertismentCost(PageViewModel request,
            CancellationToken cancellationToken);
        Task<OperationResult<IList<AdvertismentDTO>>> GetPagedAdvertismentsByUserId(
            PaginationByUserId request, CancellationToken cancellationToken);
        Task<OperationResult<bool>> DeleteAdvertismentsByUserId(string userId,
            CancellationToken cancellationToken);
        Task<OperationResult<bool>> DeleteCommentsByUserId(string userId,
            CancellationToken cancellationToken);
        Task<OperationResult<bool>> ApproveAdvertisment(int id, CancellationToken cancellationToken);
        Task<OperationResult<bool>> RejectAdvertisment(RejectionModel model, CancellationToken cancellationToken);
        Task<OperationResult<int>> GetMyPagesCount(MyPaginationModel model, CancellationToken cancellationToken);
        Task<OperationResult<int>> GetUserIdAdvertismentsPagesCount(PaginationByUserId request,
            CancellationToken cancellationToken);
    }
}
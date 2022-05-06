using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SolaraNet.DataAccessLayer.Entities;

namespace SolaraNet.DataAccessLayer.Abstracts
{
    public interface IAdvertismentRepository
    {

        Task CreateNewAdvertisment(DBAdvertisment advertismentToBeAdded, CancellationToken cancellationToken);
        Task<bool> DeleteAdvertisment(int id, bool byAdmin, CancellationToken cancellationToken);
        Task<bool> UpdateAdvertisment(int id, DBAdvertisment newAdvertisment, CancellationToken cancellationToken);
        Task<DBAdvertisment> GetAdvertismentById(int id, CancellationToken cancellationToken);
        Task<bool> AddComment(int advertismentId, DBComment comment, CancellationToken cancellationToken);
        Task<DBComment> GetCommentByWhere(Expression<Func<DBComment, bool>> commentExpression, CancellationToken cancellationToken);
        Task<bool> DeleteComment(int commentId, CancellationToken cancellationToken);

        Task<List<DBAdvertisment>> GetPaged(PaginationModel model, CancellationToken cancellationToken);

        Task<int> AdvertismentsCount(Expression<Func<DBAdvertisment, bool>> predicate,
            CancellationToken cancellationToken);

        Task<int> CommentsCount(Expression<Func<DBComment, bool>> predicate, CancellationToken cancellationToken);

        Task<IEnumerable<DBComment>> GetPagedComments(Expression<Func<DBComment, bool>> predicate, int offset,
            int limit, CancellationToken cancellationToken);

        Task<IEnumerable<DBCategory>> GetCategories(CancellationToken cancellationToken);

        Task<IEnumerable<DBCategory>> GetUndercategoryOfCategoriesByCategoryId(int parentCategoryId,
            CancellationToken cancellationToken);

        Task<DBCategory> GetCategoryById(int id, CancellationToken cancellationToken);
    }
}

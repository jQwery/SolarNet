using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using SolaraNet.DataAccessLayer.EntityFramework.Repositories;
using SolaraNet.DataAccessLayer.Abstracts;
using SolaraNet.DataAccessLayer.Entities;
using SolaraNet.DataAccessLayer.EntityFramework.DBContext;
using Microsoft.EntityFrameworkCore;

namespace SolaraNet.DataAccessLayer.EntityFramework.Repositories
{
    /// <summary>
    /// AdvertismentRepository - часть с комментариями
    /// </summary>
    public partial class AdvertismentRepository // да, это спорное решение, но для написания следующих методов удобно использовать методы репозитория с объявлениями, потому было принято решение не делать ещё один репозиторий, а просто сделать partial класс
    {
        public async Task<bool> AddComment(int advertismentId, DBComment comment, CancellationToken cancellationToken)
        {
            try
            {
                var advertisment = await GetAdvertismentById(advertismentId, cancellationToken);
                advertisment.Comments.Add(comment);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<DBComment> GetCommentByWhere(Expression<Func<DBComment, bool>> commentExpression, CancellationToken cancellationToken)
        {
            var comments = _dbContext.Set<DBComment>();
            return comments.Where(commentExpression.Compile()).FirstOrDefault();
        }

        public async Task<int> CommentsCount(Expression<Func<DBComment, bool>> predicate, CancellationToken cancellationToken)
        {
            var data = _dbContext.Set<DBComment>();
            return await data.Where(predicate).CountAsync(cancellationToken);
        }


        public async Task<bool> DeleteComment(int commentId, CancellationToken cancellationToken)
        {
            var comments = _dbContext.Set<DBComment>();
            if (! await Task.FromResult(IsCommentListValid(comments, commentId, cancellationToken)))
            {
                return false; // комментария с таким id попросту нет или же он уже был удалён
            }
            var commentToDelete = await comments.FindAsync(commentId);
            commentToDelete.CommentStatus = EntityStatus.Deleted;

            return true;
        }

        public async Task<IEnumerable<DBComment>> GetPagedComments(Expression<Func<DBComment, bool>> predicate, int offset, int limit, CancellationToken cancellationToken)
        {
            var data = _dbContext.Set<DBComment>();
            return await data.Where(predicate).OrderBy(e => e.Id).Take(limit).Skip(offset)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Проверка валидности комментария на предмет наличия его как такогового. Первый параметр - это список, в котором должен быть коммент, а второй - id коммента
        /// </summary>
        /// <param name="list"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool IsCommentListValid(IEnumerable<DBComment> list, int id, CancellationToken cancellationToken)
        {
            if (list!=null)
            {
                var dbComments = list as DBComment[] ?? list.ToArray();
                if (dbComments.Any())
                {
                    if (dbComments.FirstOrDefault(x => x.Id==id && x.CommentStatus==EntityStatus.Active)!=null)
                    {
                        return true;
                    }

                    return false;
                }

                return false;
            }

            return false;
        }

    }
}
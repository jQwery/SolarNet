using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using SolaraNet.DataAccessLayer.Entities;
using System.Threading.Tasks;

namespace SolaraNet.DataAccessLayer.Abstracts
{
    public interface IUserRepository:IDisposable
    {
        Task<bool> CreateUser(DBUser userToBeAdded, CancellationToken cancellationToken);

        Task<bool> UpdateUserEmail(Expression<Func<DBUser, bool>> userExpression, string email,
            CancellationToken cancellationToken);

        Task<bool> UpdateUserPassword(Expression<Func<DBUser, bool>> userExpression, string password,
            CancellationToken cancellationToken);
        Task<DBUser> GetUserWhere(Expression<Func<DBUser, bool>> userExpression, CancellationToken cancellationToken);
        Task<bool> DeleteUser(int id, CancellationToken cancellationToken);
    }
}
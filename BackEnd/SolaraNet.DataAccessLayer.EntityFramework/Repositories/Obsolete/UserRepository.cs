using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SolaraNet.DataAccessLayer.Abstracts;
using SolaraNet.DataAccessLayer.Entities;
using SolaraNet.DataAccessLayer.EntityFramework.DBContext;
using Microsoft.EntityFrameworkCore;
using Isopoh.Cryptography.Argon2;

namespace SolaraNet.DataAccessLayer.EntityFramework.Repositories
{
    [Obsolete("Данный класс потерял какую-либо актуальность всвязи с переходом на Identity Framework. А жаль, конечно, потому что оболденный класс был :) ")]
    public class UserRepository : IUserRepository
    {
        private readonly SolaraNetDBContext
            _dbContext;

        public UserRepository(SolaraNetDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public async Task<DBUser> GetUserWhere(Expression<Func<DBUser, bool>> userExpression, CancellationToken cancellationToken)
        {
            var compiledExpression = userExpression.Compile();
            return _dbContext.Users.Where(compiledExpression).FirstOrDefault();
        }
        public async Task<bool> CreateUser(DBUser userToBeAdded, CancellationToken cancellationToken)
        {
            try
            {
                await _dbContext.Users.AddAsync(userToBeAdded);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateUserEmail(Expression<Func<DBUser, bool>> userExpression, string email, CancellationToken cancellationToken)
        {
            try
            {
                var user = await GetUserWhere(userExpression, cancellationToken);
                user.Email = email;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UpdateUserPassword(Expression<Func<DBUser, bool>> userExpression, string password, CancellationToken cancellationToken)
        {
            try
            {
                var user = await GetUserWhere(userExpression, cancellationToken);
                user.PasswordHash = password;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteUser(int id, CancellationToken cancellationToken)
        {
            try
            {
                DBUser user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id.ToString());
                if (user != null)
                {
                    _dbContext.Users.Remove(user);
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

    }
}
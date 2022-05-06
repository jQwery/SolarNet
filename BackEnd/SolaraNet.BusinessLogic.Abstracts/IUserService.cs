using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using SolaraNet.BusinessLogic.Contracts;
using SolaraNet.BusinessLogic.Contracts.Models;

namespace SolaraNet.BusinessLogic.Abstracts
{
    public interface IUserService
    {
        Task<OperationResult<bool>> AddNewUser(UserDTO userToBeAdded, CancellationToken cancellationToken);
        Task<OperationResult<UserDTO>> GetUserById(int id, CancellationToken cancellationToken);
        Task<OperationResult<bool>> DeleteUserById(int id, CancellationToken cancellationToken);
    }
}
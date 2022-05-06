using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SolaraNet.BusinessLogic.Contracts;
using SolaraNet.BusinessLogic.Contracts.Models;
using SolaraNet.DataAccessLayer.Entities;

namespace SolaraNet.BusinessLogic.Abstracts
{
    public interface IIdentityService
    {
        Task<OperationResult<string>> GetCurrentUserId(CancellationToken cancellationToken = default);
        Task<OperationResult<bool>> IsInRole(string userId, string role, CancellationToken cancellationToken = default);
        Task<OperationResult<string>> CreateUser(UserDTO request, CancellationToken cancellationToken = default);
        Task<OperationResult<string>> CreateToken(LoginModel request, CancellationToken cancellationToken = default);
        Task<OperationResult<bool>> ConfirmEmail(string userId, string token, CancellationToken cancellationToken = default);
        Task<OperationResult<bool>> ChangeUserPassword(ChangePasswordModel model, CancellationToken cancellationToken);
        Task<OperationResult<bool>> ChangeUserEmail(ChangeEmailModel model, CancellationToken cancellationToken);
        Task<OperationResult<bool>> ChangeAvatar(string newAvatar, CancellationToken cancellationToken);
        Task<OperationResult<bool>> ChangeMobilePhoneNumber(string newNumber, CancellationToken cancellationToken);
        Task<OperationResult<UserDTO>> GetUserById(string id, CancellationToken cancellationToken);
        Task<OperationResult<bool>> ChangeUserName(string name, CancellationToken cancellationToken);
        Task<OperationResult<bool>> BanUser(string id, CancellationToken cancellationToken);
        Task<OperationResult<List<UserDTO>>> GetUsers(SimplePagination model, CancellationToken cancellationToken);
        Task<OperationResult<bool>> SendNotifications(string adminEmail, string passwordOfEmail,
            CancellationToken cancellationToken);
        Task<OperationResult<int>> GetUsersPagesCount(SimplePagination model,
            CancellationToken cancellationToken);
    }
}
using System.Threading;
using System.Threading.Tasks;
using SolaraNet.BusinessLogic.Contracts;
using SolaraNet.BusinessLogic.Contracts.Models;

namespace SolaraNet.BusinessLogic.Abstracts
{
    /// <summary>
    /// UserService на основе Identity, то есть тут всё завязано на Identity
    /// </summary>
    public interface IIdentityUserService
    {
        Task<OperationResult<string>> Register(UserDTO user, CancellationToken cancellationToken);
        Task<OperationResult<bool>> UpdateUserEmail(ChangeEmailModel model, CancellationToken cancellationToken);
        Task<OperationResult<bool>> ChangePassword(ChangePasswordModel model, CancellationToken cancellationToken);
        Task<OperationResult<bool>> ChangeUserMobilePhoneNumber(string number,
            CancellationToken cancellationToken);
        Task<OperationResult<bool>> ChangeUserName(string name, CancellationToken cancellationToken);
        Task<OperationResult<bool>> BanUser(string id, CancellationToken cancellationToken);
    }
}
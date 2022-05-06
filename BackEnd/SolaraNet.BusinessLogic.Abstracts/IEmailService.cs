using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SolaraNet.BusinessLogic.Contracts;
namespace SolaraNet.BusinessLogic.Abstracts
{
    public interface IEmailService
    {
        Task<OperationResult<bool>> SendEmailAsync(string email, string subject, string message, string adminEmail, string passwordOfAdminEmail, CancellationToken cancellationToken);
        Task<OperationResult<int>> GenerateCode(CancellationToken cancellationToken);
    }
}

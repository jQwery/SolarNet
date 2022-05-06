using System;
using System.Collections.Generic;
using MimeKit;
using MailKit.Net.Smtp;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.Execution;
using MimeKit.Text;
using SolaraNet.BusinessLogic.Abstracts;
using SolaraNet.BusinessLogic.Contracts;


namespace SolaraNet.BusinessLogic.Implementations
{
    public class EmailService:IEmailService
    {
        public async Task<OperationResult<bool>> SendEmailAsync(string email, string subject, string message, string adminEmail, string passwordOfAdminEmail, CancellationToken cancellationToken)
        {
            try
            {
                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress("Администрация сайта", adminEmail));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = message
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.yandex.ru", 25, false, cancellationToken);
                    await client.AuthenticateAsync(adminEmail, passwordOfAdminEmail, cancellationToken);
                    await client.SendAsync(emailMessage, cancellationToken);
                    await client.DisconnectAsync(true, cancellationToken);
                }

                return OperationResult<bool>.Ok(true); // успешно отправилось
            }
            catch (Exception e)
            {
                return OperationResult<bool>.Failed(new []{"По каким-то причинам в методе SendEmailAsync произошла ошибка, не удалось отправить сообщение. Подробности (осторожно, английский язык): " + e});
            }
        }

        [Obsolete("Это устаревший метод. Если вы его используете, надеюсь, знаете, что делаете.", false)]
        public async Task<OperationResult<int>> GenerateCode(CancellationToken cancellationToken)
        {
            try
            {
                Random random = default;
                await Task.Run(() =>
                {
                    random = new Random(DateTime.Now.Millisecond);
                }, cancellationToken);
                return OperationResult<int>.Ok(random.Next(0000, 9999));
            }
            catch (Exception e)
            {
                return OperationResult<int>.Failed(new []{"Не удалось сгенирировать код. Подробности: " + e});
            }
        }

    }
}

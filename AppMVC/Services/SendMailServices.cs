using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace AppMVC.Services
{
    public interface IEmailSmsSender : IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendSmsAsync(string number, string message);
    }

    public class SendMailServices : IEmailSmsSender
    {

        public class MailSettings
        {
            public string? Mail { get; set; }
            public string? DisplayName { get; set; }
            public string? Password { get; set; }
            public string? Host { get; set; }
            public int Port { get; set; }
        }

        private readonly MailSettings mailSettings;
        private readonly ILogger<SendMailServices> logger;

        public SendMailServices(IOptions<MailSettings> _mailSettings, ILogger<SendMailServices> _logger)
        {
            mailSettings = _mailSettings.Value;
            logger = _logger;
            logger.LogInformation("Create SendMailService");
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //Mail Header
            var message = new MimeMessage();
            message.Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail);
            message.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = subject;

            //Mail Body
            var builder = new BodyBuilder();
            builder.HtmlBody = htmlMessage;
            message.Body = builder.ToMessageBody();

            // Client MailKit
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                //Open Connect -> Authenticate -> SendMail
                smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
                await smtp.SendAsync(message);
            }
            catch (Exception ex)
            {
                // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
                System.IO.Directory.CreateDirectory("mailssave");
                var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                await message.WriteToAsync(emailsavefile);

                logger.LogInformation("Lỗi gửi mail, lưu tại - " + emailsavefile);
                logger.LogError(ex.Message);
            }

            smtp.Disconnect(true);
            logger.LogInformation("send mail to: " + email);
        }

        public Task SendSmsAsync(string number, string message)
        {
            Directory.CreateDirectory("smssave");
            var saveSMSFile = @$"smssave/{number}-{Guid.NewGuid()}.txt";
            System.IO.File.WriteAllTextAsync(saveSMSFile, message);
            return Task.FromResult(0);
        }

    }
}

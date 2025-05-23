using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace WebsiteQuanLyKeKhaiGiangDayCuaGiangVien.Services.EmailService
{
    public class EmailService: IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _port;
        private readonly string _senderEmail;
        private readonly string _senderPassword;

        public EmailService()
        {
            // Read email settings from web.config <appSettings>
            _smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
            _port = int.Parse(ConfigurationManager.AppSettings["Port"]);
            _senderEmail = ConfigurationManager.AppSettings["SenderEmail"];
            _senderPassword = ConfigurationManager.AppSettings["SenderPassword"];
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                using (var client = new SmtpClient(_smtpServer, _port))
                {
                    client.EnableSsl = true; // Equivalent to SecureSocketOptions.StartTls
                    client.Credentials = new System.Net.NetworkCredential(_senderEmail, _senderPassword);

                    using (var mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(_senderEmail, "Admin");
                        mailMessage.To.Add(new MailAddress(toEmail));
                        mailMessage.Subject = subject;
                        mailMessage.Body = body;
                        mailMessage.IsBodyHtml = true;

                        client.Send(mailMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using log4net or NLog in production)
                Console.WriteLine(ex.Message);
                throw; // Re-throw to let the controller handle the error
            }
        }
    }
}
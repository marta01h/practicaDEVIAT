using System;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace WpfApp9
{
    public class EmailSender
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly SecureSocketOptions _smtpSecurity;

        public EmailSender(string smtpServer, int smtpPort, SecureSocketOptions smtpSecurity)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _smtpSecurity = smtpSecurity;
        }

        public bool SendEmail(string email, string password, MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_smtpServer, _smtpPort, _smtpSecurity);
                    client.Authenticate(email, password);
                    client.Send(message);
                    client.Disconnect(true);
                    return true;
                }
                catch (Exception ex)
                {
                    // Обработка ошибок отправки
                    return false;
                }
            }
        }
    }
}

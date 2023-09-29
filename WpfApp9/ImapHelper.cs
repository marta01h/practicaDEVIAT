using System;
using MimeKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Security;
using MailKit;

namespace WpfApp9
{
    public class ImapHelper
    {
        private readonly string _imapServer;
        private readonly int _imapPort;
        private readonly SecureSocketOptions _imapSecurity;

        public ImapHelper(string imapServer, int imapPort, SecureSocketOptions imapSecurity)
        {
            _imapServer = imapServer;
            _imapPort = imapPort;
            _imapSecurity = imapSecurity;
        }

        public bool Authenticate(string email, string password)
        {
            using (var client = new ImapClient())
            {
                try
                {
                    client.Connect(_imapServer, _imapPort, _imapSecurity);
                    client.Authenticate(email, password);
                    return true;
                }
                catch (Exception ex)
                {
                    // Обработка ошибок аутентификации
                    return false;
                }
            }
        }

        public void DownloadMessages(string folderName)
        {
            using (var client = new ImapClient())
            {
                client.Connect(_imapServer, _imapPort, _imapSecurity);
                client.Authenticate("email", "password");

                var folder = client.GetFolder(folderName);
                folder.Open(FolderAccess.ReadOnly);

                for (int i = 0; i < folder.Count; i++)
                {
                    var message = folder.GetMessage(i);
                    // Обработка и отображение сообщения
                }

                client.Disconnect(true);
            }
        }
    }

    public class SmtpHelper
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly SecureSocketOptions _smtpSecurity;

        public SmtpHelper(string smtpServer, int smtpPort, SecureSocketOptions smtpSecurity)
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

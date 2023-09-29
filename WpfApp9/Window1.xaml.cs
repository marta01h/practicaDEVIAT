using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MailKit;
using MailKit.Net.Imap;
using WpfApp9;

namespace WpfApp9
{
    public partial class Window1 : Window
    {
        public ObservableCollection<MailFolderViewModel> Folders { get; set; } = new ObservableCollection<MailFolderViewModel>();
        public string Email { get; set; } = "marinatess687@gmail.com"; 
        public string Password { get; set; } = "hezhac-cytseb-dyzTy5";

        public Window1(string email, string password)
        {
            InitializeComponent();
            DataContext = this;

            // Установите email и password
            Email = email;
            Password = password;

            // Загрузите папки
            LoadFoldersAsync();

            OpenMessageButton.Click += OpenMessageButton_Click;
            ComposeMessageButton.Click += ComposeMessageButton_Click;
        }

        private async void MessagesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (messagesListView.SelectedItem is MailFolderViewModel selectedFolder)
            {
                try
                {
                    using (var client = new ImapClient())
                    {
                        await client.ConnectAsync("imap.gmail.com", 993, true);
                        await client.AuthenticateAsync(Email, Password);

                        var folder = client.GetFolder(selectedFolder.Name);
                        folder.Open(FolderAccess.ReadOnly);

                        selectedFolder.Messages.Clear(); // Очищаем список сообщений перед загрузкой новых

                        for (int i = 0; i < folder.Count; i++)
                        {
                            var message = folder.GetMessage(i);
                            var messageViewModel = new MailMessageViewModel
                            {
                                From = message.From.ToString(),
                                To = message.To.ToString(),
                                Subject = message.Subject,
                                Date = message.Date.DateTime,
                                Content = message.TextBody,
                            };
                            selectedFolder.Messages.Add(messageViewModel); // Добавляем сообщение в коллекцию сообщений
                        }

                        client.Disconnect(true);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при загрузке сообщений: {ex.Message}");
                }
            }
        }

        private async void LoadFoldersAsync()
        {
            try
            {
                using (var client = new ImapClient())
                {
                    await client.ConnectAsync("imap.gmail.com", 993, true);
                    await client.AuthenticateAsync(Email, Password);

                    var personalNamespaces = client.PersonalNamespaces;

                    foreach (var ns in personalNamespaces)
                    {
                        var rootFolder = client.GetFolder(ns.Path);

                        var folderViewModel = new MailFolderViewModel
                        {
                            Name = rootFolder.FullName,
                            SubFolders = await GetSubFoldersAsync(rootFolder)
                        };

                        Folders.Add(folderViewModel);
                    }

                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке папок: {ex.Message}");
            }
        }

        private void OpenMessageButton_Click(object sender, RoutedEventArgs e)
        {
            if (messagesListView.SelectedItem is MailMessageViewModel selectedMessage)
            {
                // Создайте объект ViewMessageWindow и передайте в него выбранное сообщение
                Window2 viewMessageWindow = new Window2(selectedMessage);
                viewMessageWindow.Show();
            }
        }

        private void ComposeMessageButton_Click(object sender, RoutedEventArgs e)
        {
            string recipient = "здесь ваш получатель"; // Замените на реальное значение
            Window3 composeMessageWindow = new Window3(recipient);
            composeMessageWindow.ShowDialog();
        }



        private async Task<List<MailFolderViewModel>> GetSubFoldersAsync(IMailFolder folder)
        {
            var subFolders = new List<MailFolderViewModel>();

            foreach (var subFolder in folder.GetSubfolders())
            {
                var subFolderViewModel = new MailFolderViewModel
                {
                    Name = subFolder.FullName,
                    SubFolders = await GetSubFoldersAsync(subFolder)
                };

                subFolders.Add(subFolderViewModel);
            }

            return subFolders;
        }
    }

    public class MailFolderViewModel
    {
        public string Name { get; set; }
        public List<MailFolderViewModel> SubFolders { get; set; } = new List<MailFolderViewModel>();
        public ObservableCollection<MailMessageViewModel> Messages { get; set; } = new ObservableCollection<MailMessageViewModel>();
    }

    public class MailMessageViewModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public DateTime Date { get; set; }
        public string Content { get; set; }
    }
}

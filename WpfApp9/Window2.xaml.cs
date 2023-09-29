using System.Windows;

namespace WpfApp9
{
    public partial class Window2 : Window
    {
        public Window2(MailMessageViewModel message)
        {
            InitializeComponent();
            senderTextBlock.Text = message.From;
            subjectTextBlock.Text = message.Subject;
            dateTextBlock.Text = message.Date.ToString();
            messageTextBox.Text = message.Content;
        }

        private void ReplyButton_Click(object sender, RoutedEventArgs e)
        {
            // Здесь добавьте код для отправки ответа на сообщение
            // Можете использовать данные полученные из message (отправитель, тема, исходное сообщение)
            // После отправки ответа закройте окно
            this.Close();
        }
    }
}

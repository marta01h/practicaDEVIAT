using System.Windows;

namespace WpfApp9
{
    public partial class Window3 : Window
    {
        public Window3(string recipient)
        {
            InitializeComponent();
            recipientTextBox.Text = recipient;
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string recipient = recipientTextBox.Text;
            string subject = subjectTextBox.Text;
            string content = messageTextBox.Text;

          
            this.Close();
        }
    }
}

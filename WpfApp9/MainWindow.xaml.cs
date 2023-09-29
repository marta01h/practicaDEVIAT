using System;
using System.Windows;
using System.Windows.Controls;
using MailKit;
using MailKit.Net.Imap;
using WpfApp9;

namespace WpfApp9
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = emailTextBox.Text;
            string password = passwordBox.Password;
            string selectedService = (emailComboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(selectedService))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            try
            {
                // Передаем адрес электронной почты и пароль в окно Window1
                Window1 window1 = new Window1(email, password);
                window1.Show();
                this.Close();

                MessageBox.Show("Авторизация прошла успешно!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при авторизации: {ex.Message}");
            }
        }
    }
}

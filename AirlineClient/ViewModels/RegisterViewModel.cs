using AirlineClient.Models;
using AirlineClient.Services;
using AirlineClient.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AirlineClient.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly AuthService _auth = new AuthService();

        private string _email = string.Empty;
        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }

        private string _fullName = string.Empty;
        public string FullName { get => _fullName; set { _fullName = value; OnPropertyChanged(); } }

        private string _role = "Диспетчер";
        public string Role { get => _role; set { _role = value; OnPropertyChanged(); } }

        public List<string> Roles { get; } = new() { "Администратор", "Диспетчер" };

        public ICommand RegisterCommand { get; }

        public RegisterViewModel()
        {
            RegisterCommand = new RelayCommand(async param => await ExecuteRegister(param));
        }

        private async Task ExecuteRegister(object? param)
        {
            var passwordBox = param as PasswordBox;
            string password = passwordBox?.Password ?? string.Empty;

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Заполните Email и пароль!", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                string result = await _auth.Register(new User
                {
                    Email = Email.Trim(),
                    Password = password,
                    FullName = string.IsNullOrWhiteSpace(FullName) ? Email : FullName.Trim(),
                    Role = Role
                });

                MessageBox.Show(result, "Регистрация", MessageBoxButton.OK, MessageBoxImage.Information);
                if (result.Contains("успешно"))
                {
                    foreach (Window win in Application.Current.Windows)
                    {
                        if (win.GetType().Name == "RegisterWindow")
                        {
                            win.Close();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к API: " + ex.Message);
            }
        }
    }
}

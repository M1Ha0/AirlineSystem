using AirlineClient.Models;
using AirlineClient.Services;
using AirlineClient.Utils;
using AirlineClient.Views;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AirlineClient.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly AuthService _auth = new AuthService();

        private string _email = "admin@avia.ru";
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(async param => await ExecuteLogin(param));
        }

        private async Task ExecuteLogin(object? param)
        {
            ErrorMessage = string.Empty;
            var passwordBox = param as PasswordBox;
            string password = passwordBox?.Password ?? string.Empty;

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(password))
            {
                ErrorMessage = "Введите логин и пароль";
                return;
            }

            try
            {
                var resp = await _auth.SignIn(new User { Email = Email.Trim(), Password = password });
                if (resp == null || string.IsNullOrEmpty(resp.access_token))
                {
                    ErrorMessage = "Неверный логин или пароль";
                    return;
                }

                SessionState.AccessToken = resp.access_token;
                SessionState.UserEmail = resp.username;
                SessionState.UserRole = resp.role;
                SessionState.UserFullName = resp.fullName;

                var mainWin = new MainWindow();
                mainWin.Show();

                foreach (Window win in Application.Current.Windows)
                {
                    if (win is LoginWindow)
                    {
                        win.Close();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Сервер API недоступен: " + ex.Message;
            }
        }
    }
}

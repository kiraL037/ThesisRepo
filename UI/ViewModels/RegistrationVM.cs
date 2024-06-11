using Core.Interfaces;
using NLog;
using Services.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UI.ViewModels;
using UI.Views.Windows;

namespace UI.ViewModels
{
    public class RegistrationVM : ViewModelBase, INotifyPropertyChanged
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IUserService _userService;
        private readonly ISecurityMethods _securityMethods;
        private string _username;
        private string _password;
        private string _confirmPassword;

        public ICommand RegisterCommand => new RelayCommand(async _ => await Register());

        public RegistrationVM(IUserService userService, ISecurityMethods securityMethods)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _securityMethods = securityMethods ?? throw new ArgumentNullException(nameof(securityMethods));
        }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }

        public new event PropertyChangedEventHandler PropertyChanged;

        private new void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task Register()
        {
            try
            {
                if (Password.Length < 8 || !IsPasswordComplex(Password))
                {
                    MessageBox.Show("Пароль должен содержать хотя бы одну цифру, заглавную и строчную буквы, специальный символ");
                    return;
                }

                if (Password != ConfirmPassword)
                {
                    MessageBox.Show("Не совпадает пароль");
                    return;
                }

                string salt = _securityMethods.GenerateSalt();
                var passwordHash = _securityMethods.HashPassword(Password, salt);

                var result = await _userService.RegisterUserAsync(Username, passwordHash, salt, false);

                if (result)
                {
                    MessageBox.Show("Пользователь зарегистрирован");
                    CloseWindow();
                    AuthWindow();
                }
                else
                {
                    MessageBox.Show("Ошибка регистрации пользователя");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Неожиданная ошибка во время регистрации");
                MessageBox.Show($"Неожиданная ошибка во время регистрации: {ex.Message}");
            }
        }


        private bool IsPasswordComplex(string password)
        {
            bool hasNumber = false, hasUpperChar = false, hasSpecialChar = false;
            foreach (char c in password)
            {
                if (char.IsDigit(c)) hasNumber = true;
                else if (char.IsUpper(c)) hasUpperChar = true;
                else if (!char.IsLetterOrDigit(c)) hasSpecialChar = true;
            }
            return hasNumber && hasUpperChar && hasSpecialChar;
        }

        private void AuthWindow()
        {
            var viewModel = new AuthenticationWindowVM(_userService);
            var authWindow = new AuthenticationWindow(viewModel);
            authWindow.Show();
            CloseWindow();
        }

        private void CloseWindow()
        {
            Application.Current.Windows.OfType<RegistrationWindow>().FirstOrDefault()?.Close();
        }

        private RelayCommand goBack;

        public ICommand GoBack
        {
            get
            {
                if (goBack == null)
                {
                    goBack = new RelayCommand(PerformGoBack);
                }

                return goBack;
            }
        }

        private void PerformGoBack(object commandParameter)
        {
            var viewModel = new AuthenticationWindowVM(_userService);
            var authWindow = new AuthenticationWindow(viewModel);
            authWindow.Show();
            CloseWindow();
        }
    }
}
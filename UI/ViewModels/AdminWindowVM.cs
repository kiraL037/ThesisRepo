using Core.Interfaces;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UI.Views.Windows;

namespace UI.ViewModels
{
    public class AdminWindowVM : ViewModelBase
    {
        private readonly IUserService _userService;
        private readonly ISecurityMethods _securityMethods;
        private string _username;
        private string _password;
        private string _confirmPassword;

        public ICommand RegisterCommand => new RelayCommand(async _ => await Register());

        public AdminWindowVM(IUserService userService, ISecurityMethods securityMethods)
        {
            _userService = userService;
            _securityMethods = securityMethods;
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

        private async Task Register()
        {
            try
            {
                // Validate password length and complexity
                if (Password.Length < 8 || !IsPasswordComplex(Password))
                {
                    MessageBox.Show("Password must be at least 8 characters long and contain a number, an uppercase letter, and a special character.");
                    return;
                }

                // Confirm password match
                if (Password != ConfirmPassword)
                {
                    MessageBox.Show("Passwords do not match.");
                    return;
                }

                string salt = _securityMethods.GenerateSalt();
                var passwordHash = _securityMethods.HashPassword(Password, salt);

                var result = await _userService.RegisterUserAsync(Username, passwordHash, salt, true);

                if (result)
                {
                    MessageBox.Show("Manager registered successfully.");
                    CloseWindow();
                    OpenWelcomeWindow();
                }
                else
                {
                    MessageBox.Show("Failed to register manager.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred during registration: {ex.Message}");
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

        private void OpenWelcomeWindow()
        {
            var viewModel = new WelcomeWindowVM();
            var welcomeWindow = new WelcomeWindow(viewModel);
            welcomeWindow.Show();
            CloseWindow();
        }

        private void CloseWindow()
        {
            Application.Current.Windows.OfType<AdminWindow>().FirstOrDefault()?.Close();
        }
    }
}
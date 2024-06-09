using Core.Interfaces;
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
using ThesisProjectARM.Core.Interfaces;
using ThesisProjectARM.Services.Services;
using ThesisProjectARM.UI.Views.Windows;

namespace ThesisProjectARM.UI.ViewModels
{
    public class RegistrationVM : ViewModelBase, INotifyPropertyChanged
    {
        private readonly IUserService _userService;
        private readonly ISecurityMethods _securityMethods;
        private string _username;
        private string _password;
        private string _confirmPassword;
        public ICommand RegisterCommand => new RelayCommand(async _ => await Register());

        public RegistrationVM(IUserService userService, ISecurityMethods securityMethods)
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

        public new event PropertyChangedEventHandler PropertyChanged;

        private new void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task Register()
        {
            if (Password.Length < 8 || !IsPasswordComplex(Password))
            {
                MessageBox.Show("Password must be at least 8 characters long and contain a number, an uppercase letter, and a special character.");
                return;
            }

            if (Password != ConfirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            var passwordHash = _securityMethods.HashPassword(Password, out string salt);
            var result = await _userService.RegisterUserAsync(Username, passwordHash, false);

            if (result)
            {
                MessageBox.Show("User registered successfully.");
                CloseWindow();
                // Открытие окна аутентификации после успешной регистрации
                AuthWindow();
            }
            else
            {
                MessageBox.Show("Failed to register user.");
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
            var authWindow = new AuthenticationWindow();
            authWindow.Show();
            CloseWindow();
        }

        private void CloseWindow()
        {
            Application.Current.Windows.OfType<RegistrationWindow>().FirstOrDefault()?.Close();
        }
    }
}
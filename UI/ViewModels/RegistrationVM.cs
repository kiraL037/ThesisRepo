using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ThesisProjectARM.Core.Interfaces;
using ThesisProjectARM.UI.Views.Windows;

namespace ThesisProjectARM.UI.ViewModels
{
    public class RegistrationVM : ViewModelBase, INotifyPropertyChanged
    {
        private readonly IUserService _userService;
        private string _username;
        private string _password;
        private string _confirmPassword; // Добавлено поле для подтверждения пароля
        private bool _isAdmin;

        public RegistrationVM(IUserService userService)
        {
            _userService = userService;
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string ConfirmPassword // Добавлено свойство для подтверждения пароля
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
            }
        }

        public bool IsAdmin
        {
            get => _isAdmin;
            set
            {
                _isAdmin = value;
                OnPropertyChanged();
            }
        }

        public ICommand RegisterCommand => new RelayCommand(async (param) => await RegisterUser());

        private async Task RegisterUser()
        {
            bool result = await _userService.RegisterUserAsync(Username, Password, IsAdmin); // Ошибка исправлена
            if (result)
            {
                // Логика после успешной регистрации
                MessageBox.Show("Registration successful. Please log in.", "Registration Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                CloseWindow();
                AuthenticationWindow loginWindow = new AuthenticationWindow();
                loginWindow.Show();
            }
            else
            {
                // Логика при неуспешной регистрации
                MessageBox.Show("Registration failed. Please check your details.", "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                    break;
                }
            }
        }

        public new event PropertyChangedEventHandler PropertyChanged;

        protected new virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string this[string columnName]
        {
            get
            {
                string result = null;
                switch (columnName)
                {
                    case nameof(Username):
                        if (string.IsNullOrWhiteSpace(Username))
                            result = "Логин не может быть пустым";
                        break;
                    case nameof(Password):
                        if (string.IsNullOrWhiteSpace(Password))
                            result = "Пароль не может быть пустым";
                        break;
                    case nameof(ConfirmPassword):
                        if (ConfirmPassword != Password)
                            result = "Пароли не совпадают";
                        break;
                }
                return result;
            }
        }

        public string Error => null;
    }
}
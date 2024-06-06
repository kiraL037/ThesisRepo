using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UI.Views.Windows;

namespace ThesisProjectARM.UI.ViewModels
{
    public class RegistrationVM : INotifyPropertyChanged
    {
        private readonly IUserService _userService;
        private string _username;
        private string _password;
        private bool _isAdmin;

        public RegisterViewModel(IUserService userService)
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

        public bool IsAdmin
        {
            get => _isAdmin;
            set
            {
                _isAdmin = value;
                OnPropertyChanged();
            }
        }

        public ICommand RegisterCommand => new RelayCommand(async () => await RegisterUser());

        private async Task RegisterUser()
        {
            var result = await _userService.RegisterUserAsync(Username, Password, IsAdmin);
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
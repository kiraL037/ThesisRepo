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
using ThesisProjectARM.Services.Services;
using ThesisProjectARM.UI.Views.Windows;

namespace ThesisProjectARM.UI.ViewModels
{
    public class RegistrationVM : ViewModelBase, INotifyPropertyChanged
    {
        private readonly IUserService _userService;
        private string _username;
        private string _password;
        private string _confirmPassword;

        public RegistrationWindowVM(IUserService userService)
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

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
            }
        }

        public ICommand RegisterCommand => new RelayCommand(async _ => await Register());

        private async Task Register()
        {
            if (Password != ConfirmPassword)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            var result = await _userService.RegisterUserAsync(Username, Password);

            if (result)
            {
                MessageBox.Show("User registered successfully.");
                CloseWindow();
            }
            else
            {
                MessageBox.Show("Failed to register user.");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CloseWindow()
        {
            Application.Current.Windows.OfType<RegistrationWindow>().FirstOrDefault()?.Close();
        }
    }
}
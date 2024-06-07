using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using ThesisProjectARM.Core.Interfaces;
using ThesisProjectARM.Core.Models;
using System.Data;
using ThesisProjectARM.UI.Views.Windows;
using System.Windows;

namespace ThesisProjectARM.UI.ViewModels
{
    public class AuthenticationWindowVM : INotifyPropertyChanged
    {
        private readonly IUserService _userService;
        private string _username;
        private string _password;
        private bool _isAuthenticated;

        public AuthenticationWindowVM(IUserService userService)
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

        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set
            {
                _isAuthenticated = value;
                OnPropertyChanged();
            }
        }

        public ICommand AuthenticateCommand => new RelayCommand(async (param) => await AuthenticateUser());

        private async Task AuthenticateUser()
        {
            IsAuthenticated = await _userService.AuthenticateUserAsync(Username, Password);
            if (IsAuthenticated)
            {
                // Логика при успешной аутентификации
                MainUIWindow mainUIWindow = new MainUIWindow();
                mainUIWindow.Show();
                // Закрытие окна аутентификации
                CloseWindow();
            }
            else
            {
                // Логика при неуспешной аутентификации
                MessageBox.Show("Invalid username or password.", "Authentication Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseWindow()
        {
            // Закрытие текущего окна
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
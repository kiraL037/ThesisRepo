using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using ThesisProjectARM.Core.Interfaces;
using ThesisProjectARM.Core.Models;
using System.Data;
using ThesisProjectARM.UI.Views.Windows;
using System.Windows;
using ThesisProjectARM.Services.Services;
using System.Linq;

namespace ThesisProjectARM.UI.ViewModels
{
    public class AuthenticationWindowVM : INotifyPropertyChanged
    {
        private readonly IUserService _userService;
        private string _username;
        private string _password;

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

        public ICommand LoginCommand => new RelayCommand(async _ => await Login());

        private async Task Login()
        {
            var result = await _userService.AuthenticateUserAsync(Username, Password);

            if (result)
            {
                MessageBox.Show("User authenticated successfully.");
                CloseWindow();
            }
            else
            {
                MessageBox.Show("Failed to authenticate user.");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void CloseWindow()
        {
            Application.Current.Windows.OfType<AuthenticationWindow>().FirstOrDefault()?.Close();
        }
    }
}
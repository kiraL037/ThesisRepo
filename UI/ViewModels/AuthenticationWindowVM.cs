using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Core.Interfaces;
using UI.Views.Windows;
using System.Windows;
using System.Linq;
using System.Security;
using System;

namespace UI.ViewModels
{
    public class AuthenticationWindowVM : INotifyPropertyChanged
    {
        private readonly IUserService _userService;
        private string _username;
        private SecureString _password;

        public AuthenticationWindowVM(IUserService userService)
        {
            _userService = userService;
        }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public SecureString Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand => new RelayCommand(async _ => await Login());

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task Login()
        {
            var result = await _userService.AuthenticateUserAsync(Username, ConvertToUnsecureString(Password));

            if (result)
            {
                MessageBox.Show("Login successful.");
                OpenMainUIWindow();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        private string ConvertToUnsecureString(SecureString securePassword)
        {
            if (securePassword == null)
                throw new ArgumentNullException(nameof(securePassword));

            var unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return System.Runtime.InteropServices.Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        private void OpenMainUIWindow()
        {
            var mainWindow = new MainUIWindow();
            mainWindow.Show();
            CloseWindow();
        }

        private void CloseWindow()
        {
            Application.Current.Windows.OfType<AuthenticationWindow>().FirstOrDefault()?.Close();
        }
    }
}
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Core.Interfaces;
using UI.ViewModels;
using UI.Views.Windows;
using System.Windows;
using System.Linq;
using System.Security;
using System;
using Services.Services;
using Data.Repositories;
using NLog;
using UI.Properties;

namespace UI.ViewModels
{
    public class AuthenticationWindowVM : INotifyPropertyChanged
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IUserService _userService;
        private string _username;
        private string _password;

        public AuthenticationWindowVM(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
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

        public string Password
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
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            try
            {
                var result = await _userService.AuthenticateUserAsync(Username, Password);

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
            catch (Exception ex)
            {
                Logger.Error(ex, "Error during login process.");
                MessageBox.Show($"Error during login process: {ex.Message}");
            }
        }

        private void OpenMainUIWindow()
        {
            var viewModel = new MainUIVM();
            var mainUIWindow = new MainUIWindow(viewModel);
            mainUIWindow.Show();
            CloseWindow();
        }

        private void CloseWindow()
        {
            Application.Current.Windows.OfType<AuthenticationWindow>().FirstOrDefault()?.Close();
        }

        private RelayCommand openRegistrationWindow;

        public ICommand OpenRegistrationWindow
        {
            get
            {
                if (openRegistrationWindow == null)
                {
                    openRegistrationWindow = new RelayCommand(PerformOpenRegistrationWindow);
                }

                return openRegistrationWindow;
            }
        }

        public void PerformOpenRegistrationWindow(object commandParameter)
        {
            var userRepository = new UserRepository(Settings.Default.ConnectionString);
            var securityMethods = new SecurityMethods();
            var userService = new UserService(userRepository, securityMethods);

            var registrationVM = new RegistrationVM(userService, securityMethods);
            var registrationWindow = new RegistrationWindow(registrationVM);
            registrationWindow.ShowDialog();
        }
    }
}
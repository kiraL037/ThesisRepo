using Core.Interfaces;
using System.Windows;
using System.Windows.Input;
using UI.Views.Windows;

namespace UI.ViewModels
{
    public class WelcomeWindowVM : ViewModelBase
    {
        public ICommand StartCommand { get; }
        public ICommand ShowTipsCommand { get; }
        private readonly IUserService _userService;

        public WelcomeWindowVM(IUserService userService)
        {
            _userService = userService;
        }

        public WelcomeWindowVM()
        {
            StartCommand = new RelayCommand(OnStart);
            ShowTipsCommand = new RelayCommand(OnShowTips);
        }

        private string _welcomeMessage;
        public string WelcomeMessage
        {
            get { return _welcomeMessage; }
            set
            {
                _welcomeMessage = value;
                OnPropertyChanged(nameof(WelcomeMessage));
            }
        }

        private void OnStart(object parameter)
        {
            var viewModel = new AuthenticationWindowVM(_userService);
            var authWindow = new AuthenticationWindow(viewModel);
            authWindow.Show();
            Application.Current.MainWindow.Close();
        }

        private void OnShowTips(object parameter)
        {
            var viewModel = new TipsWindowVM();
            var tipsWindow = new TipsWindow(viewModel);
            tipsWindow.Show();
            Application.Current.MainWindow.Close();
        }
    }
}
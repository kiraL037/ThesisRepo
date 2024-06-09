using System.Windows;
using System.Windows.Input;
using UI.Views.Windows;

namespace UI.ViewModels
{
    public class WelcomeWindowVM : ViewModelBase
    {
        public ICommand StartCommand { get; }
        public ICommand ShowTipsCommand { get; }

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
            // Открываем окно авторизации
            var authWindow = new AuthenticationWindow();
            authWindow.Show();
            Application.Current.MainWindow.Close();
        }

        private void OnShowTips(object parameter)
        {
            // Открываем окно подсказок
            var tipsWindow = new TipsWindow();
            tipsWindow.Show();
            Application.Current.MainWindow.Close();
        }
    }
}
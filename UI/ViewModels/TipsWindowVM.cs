using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using UI.Views.Windows;
using System.Linq;

namespace UI.ViewModels
{
    public class TipsWindowVM : ViewModelBase
    {
        public ICommand BackToWelcomeCommand { get; }

        private ObservableCollection<string> _tips;
        public ObservableCollection<string> Tips
        {
            get { return _tips; }
            set { _tips = value; OnPropertyChanged(nameof(Tips)); }
        }

        public TipsWindowVM()
        {
            BackToWelcomeCommand = new RelayCommand(BackToWelcome);

            Tips = new ObservableCollection<string>
            {
                "Tip 1",
                "Tip 2",
                "Tip 3"
            };
        }

        private void BackToWelcome(object parameter)
        {
            var viewModel = new WelcomeWindowVM();
            var welcomeWindow = new WelcomeWindow(viewModel);
            welcomeWindow.Show();
            CloseWindow();
        }

        private void CloseWindow()
        {
            Application.Current.Windows.OfType<TipsWindow>().FirstOrDefault()?.Close();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using ThesisProjectARM.UI.Views.Windows;

namespace ThesisProjectARM.UI.ViewModels
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
            "Tip 1: Follow the instructions.",
            "Tip 2: Save your work regularly.",
            "Tip 3: Ensure data is backed up."
        };
        }

        private void BackToWelcome(object parameter)
        {
            var welcomeWindow = new WelcomeWindow();
            welcomeWindow.Show();
            Application.Current.MainWindow.Close();
        }
    }
}
using System.Windows;
using System.Windows.Controls;
using Core.Models;
using UI.ViewModels;

namespace UI.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для FirstSetupWindow.xaml
    /// </summary>
    public partial class FirstSetupWindow : Window
    {
        private readonly FirstSetupWindowVM _viewModel;

        public FirstSetupWindow(FirstSetupWindowVM viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is FirstSetupWindowVM viewModel)
            {
                viewModel.Password = ((PasswordBox)sender).Password;
            }
        }
    }
}
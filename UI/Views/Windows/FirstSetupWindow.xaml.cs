using System.Windows;
using System.Windows.Controls;
using ThesisProjectARM.Core.Models;
using ThesisProjectARM.UI.ViewModels;

namespace ThesisProjectARM.UI.Views.Windows
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

        public void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegistrationVM viewModel)
            {
                viewModel.Password = ((PasswordBox)sender).Password;
            }
        }
    }
}

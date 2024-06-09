using System.Windows;
using System.Windows.Controls;
using ThesisProjectARM.UI.ViewModels;

namespace ThesisProjectARM.UI.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        public void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegistrationVM viewModel)
            {
                viewModel.Password = ((PasswordBox)sender).Password;
            }
        }

        public void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegistrationVM viewModel)
            {
                viewModel.ConfirmPassword = ((PasswordBox)sender).Password;
            }
        }
    }
}
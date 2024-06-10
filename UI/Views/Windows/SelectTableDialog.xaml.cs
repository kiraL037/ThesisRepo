using System.Windows;
using UI.ViewModels;

namespace UI.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для SelectTableDialog.xaml
    /// </summary>
    public partial class SelectTableDialog : Window
    {
        private readonly SelectTableVM _viewModel;

        public SelectTableDialog(SelectTableVM viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void WindowsAuthCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ((SelectTableVM)DataContext).IsUserEnabled = false;
            ((SelectTableVM)DataContext).IsPasswordEnabled = false;
        }

        private void WindowsAuthCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ((SelectTableVM)DataContext).IsUserEnabled = true;
            ((SelectTableVM)DataContext).IsPasswordEnabled = true;
        }
    }
}

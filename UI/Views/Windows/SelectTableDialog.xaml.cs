using System.Windows;
using UI.ViewModels;

namespace UI.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для SelectTableDialog.xaml
    /// </summary>
    public partial class SelectTableDialog : Window
    {
        public SelectTableDialog()
        {
            InitializeComponent();
            DataContext = new SelectTableVM();
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

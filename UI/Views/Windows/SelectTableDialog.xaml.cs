using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ThesisProjectARM.UI.ViewModels;

namespace ThesisProjectARM.UI.Views.Windows
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

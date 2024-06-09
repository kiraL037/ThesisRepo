using System.Windows;
using UI.ViewModels;

namespace UI.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        public ManagerWindow()
        {
            InitializeComponent();
            DataContext = new ManagerVM();
        }
    }
}

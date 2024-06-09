using System.Windows;
using UI.ViewModels;

namespace UI.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для TipsWindow.xaml
    /// </summary>
    public partial class TipsWindow : Window
    {
        public TipsWindow()
        {
            InitializeComponent();
            DataContext = new TipsWindowVM();
        }
    }
}

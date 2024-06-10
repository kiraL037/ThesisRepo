using System.Windows;
using UI.ViewModels;

namespace UI.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для TipsWindow.xaml
    /// </summary>
    public partial class TipsWindow : Window
    {
        private readonly TipsWindowVM _viewModel;

        public TipsWindow(TipsWindowVM viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }
    }
}

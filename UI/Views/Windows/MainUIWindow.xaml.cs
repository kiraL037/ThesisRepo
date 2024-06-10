using System.Windows;
using UI.ViewModels;
using UI;

namespace UI.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainUIWindow.xaml
    /// </summary>
    public partial class MainUIWindow : Window
    {
        private readonly MainUIVM _viewModel;

        public MainUIWindow(MainUIVM viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }
    }
}

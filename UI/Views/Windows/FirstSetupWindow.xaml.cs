using System.Windows;
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
    }
}

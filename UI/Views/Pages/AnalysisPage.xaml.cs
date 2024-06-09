using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows.Controls;
using UI.ViewModels;

namespace UI.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для AnalysisPage.xaml
    /// </summary>
    public partial class AnalysisPage : Page
    {
        private AnalysisVM viewModel;

        public AnalysisPage()
        {
            InitializeComponent();
            viewModel = DataContext as AnalysisVM;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItems = ((ListBox)sender).SelectedItems;
            viewModel.SelectedColumns = new ObservableCollection<string>(selectedItems.Cast<string>());
        }
    }
}
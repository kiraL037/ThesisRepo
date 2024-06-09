using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ThesisProjectARM.UI.ViewModels;

namespace ThesisProjectARM.UI.Views.Pages
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
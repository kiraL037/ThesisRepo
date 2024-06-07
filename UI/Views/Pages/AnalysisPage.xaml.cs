using System;
using System.Collections.Generic;
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
        private readonly AnalysisVM analysisViewModel;

        public AnalysisPage()
        {
            InitializeComponent();

            var classificationClustering = new ClassificationClustering(); // Предполагаем, что этот класс определен где-то
            var regressionAnalysis = new RegressionAnalysis(); // Предполагаем, что этот класс определен где-то
            analysisViewModel = new AnalysisVM(classificationClustering, regressionAnalysis);
            DataContext = analysisViewModel;
        }

        public void LoadData(DataTable dataTable)
        {
            analysisViewModel.SelectedData = dataTable;
        }

        private void AnalysisColumnsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            analysisViewModel.SelectedColumns.Clear();
            foreach (var item in analysisColumnsListBox.SelectedItems)
            {
                analysisViewModel.SelectedColumns.Add(item as string);
            }
        }
    }
}
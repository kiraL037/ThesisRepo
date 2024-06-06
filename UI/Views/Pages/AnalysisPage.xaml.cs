using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
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

            var classificationClustering = new ClassificationClustering();
            var regressionAnalysis = new RegressionAnalysis();
            analysisViewModel = new AnalysisVM(classificationClustering, regressionAnalysis);
            DataContext = analysisViewModel;
        }

        public void LoadData(DataTable dataTable)
        {
            analysisViewModel.SelectedData = dataTable;
        }
    }
}

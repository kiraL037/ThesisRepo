using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ThesisProjectARM.Core.Interfaces;
using ThesisProjectARM.Services.Services.Analyzes;

namespace ThesisProjectARM.UI.ViewModels
{
    public class AnalysisVM : ViewModelBase
    {
        private readonly IClassificationClustering classificationClusteringService;
        private readonly IRegressionAnalysis regressionAnalysisService;
        private readonly StatisticalAnalysis statisticalAnalysisService;
        private readonly TimeSeriesAnalysis timeSeriesAnalysisService;
        private DataTable _selectedData;
        private ObservableCollection<string> _selectedColumns;

        public AnalysisVM()
        {
            classificationClusteringService = new ClassificationClustering();
            regressionAnalysisService = new RegressionAnalysis();
            statisticalAnalysisService = new StatisticalAnalysis();
            timeSeriesAnalysisService = new TimeSeriesAnalysis();
            _selectedColumns = new ObservableCollection<string>();
        }

        public DataTable SelectedData
        {
            get => _selectedData;
            set
            {
                _selectedData = value;
                OnPropertyChanged(nameof(SelectedData));
            }
        }

        public ObservableCollection<string> SelectedColumns
        {
            get => _selectedColumns;
            set
            {
                _selectedColumns = value;
                OnPropertyChanged(nameof(SelectedColumns));
            }
        }

        public async Task PerformKMeansClusteringAsync(string[] columnNames, int clusterCount)
        {
            var clusters = await classificationClusteringService.KMeansClusteringAsync(SelectedData, columnNames, clusterCount);
        }

        public async Task PerformRegressionAnalysisAsync(string dependentVariable, string[] independentVariables, float[] newValues)
        {
            var prediction = await regressionAnalysisService.PredictAsync(SelectedData, dependentVariable, independentVariables, newValues);
        }

        public async Task<double> CalculateCorrelationAsync(string column1, string column2)
        {
            return await Task.Run(() => CorrelationAnalysis.CalculateCorrelation(SelectedData, column1, column2));
        }

        public async Task<double> CalculateMeanAsync(string columnName)
        {
            return await Task.Run(() => statisticalAnalysisService.CalculateMean(SelectedData, columnName));
        }

        public async Task<double> CalculateMedianAsync(string columnName)
        {
            return await Task.Run(() => statisticalAnalysisService.CalculateMedian(SelectedData, columnName));
        }

        public async Task<double> CalculateStandardDeviationAsync(string columnName)
        {
            return await Task.Run(() => statisticalAnalysisService.CalculateStandardDeviation(SelectedData, columnName));
        }

        public async Task<double[]> ForecastTimeSeriesAsync(string columnName, int periods)
        {
            return await Task.Run(() => timeSeriesAnalysisService.Forecast(SelectedData, columnName, periods));
        }

        public async Task<(double[] seasonal, double[] trend)> DetectSeasonalPatternsAsync(string columnName)
        {
            return await Task.Run(() => timeSeriesAnalysisService.SeasonalCyclicPatterns(SelectedData, columnName));
        }
    }
}
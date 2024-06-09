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
        private readonly IClassificationClustering _classificationClusteringService;
        private readonly IRegressionAnalysis _regressionAnalysisService;
        private readonly StatisticalAnalysis _statisticalAnalysisService;
        private readonly TimeSeriesAnalysis _timeSeriesAnalysisService;
        private DataTable _dataTable;
        private ObservableCollection<string> _selectedColumns;
        private string _analysisResult;

        public AnalysisVM(DataTable dataTable)
        {
            _classificationClusteringService = new ClassificationClustering();
            _regressionAnalysisService = new RegressionAnalysis();
            _statisticalAnalysisService = new StatisticalAnalysis();
            _timeSeriesAnalysisService = new TimeSeriesAnalysis();
            _dataTable = dataTable;
            AvailableColumns = new ObservableCollection<string>(_dataTable.Columns.Cast<DataColumn>().Select(col => col.ColumnName));
            _selectedColumns = new ObservableCollection<string>();

            PerformKMeansClusteringCommand = new RelayCommand(async (param) => await PerformKMeansClusteringAsync(3), CanExecuteAnalysis);
            PerformRegressionAnalysisCommand = new RelayCommand(async (param) => await PerformRegressionAnalysisAsync("DependentVariable", new float[] { 1.0f, 2.0f }), CanExecuteAnalysis);
            CalculateCorrelationCommand = new RelayCommand(async (param) => await CalculateCorrelationAsync("Column1", "Column2"), CanExecuteAnalysis);
            CalculateMeanCommand = new RelayCommand(async (param) => await CalculateMeanAsync("ColumnName"), CanExecuteAnalysis);
            CalculateMedianCommand = new RelayCommand(async (param) => await CalculateMedianAsync("ColumnName"), CanExecuteAnalysis);
            CalculateStandardDeviationCommand = new RelayCommand(async (param) => await CalculateStandardDeviationAsync("ColumnName"), CanExecuteAnalysis);
            ForecastTimeSeriesCommand = new RelayCommand(async (param) => await ForecastTimeSeriesAsync("ColumnName", 12), CanExecuteAnalysis);
            DetectSeasonalPatternsCommand = new RelayCommand(async (param) => await DetectSeasonalPatternsAsync("ColumnName"), CanExecuteAnalysis);
        }

        public ObservableCollection<string> AvailableColumns { get; }

        public ObservableCollection<string> SelectedColumns
        {
            get => _selectedColumns;
            set
            {
                _selectedColumns = value;
                OnPropertyChanged(nameof(SelectedColumns));
            }
        }

        public string AnalysisResult
        {
            get => _analysisResult;
            set
            {
                _analysisResult = value;
                OnPropertyChanged(nameof(AnalysisResult));
            }
        }

        public ICommand PerformKMeansClusteringCommand { get; }
        public ICommand PerformRegressionAnalysisCommand { get; }
        public ICommand CalculateCorrelationCommand { get; }
        public ICommand CalculateMeanCommand { get; }
        public ICommand CalculateMedianCommand { get; }
        public ICommand CalculateStandardDeviationCommand { get; }
        public ICommand ForecastTimeSeriesCommand { get; }
        public ICommand DetectSeasonalPatternsCommand { get; }

        private bool CanExecuteAnalysis(object parameter)
        {
            return SelectedColumns != null && SelectedColumns.Any();
        }

        private async Task PerformKMeansClusteringAsync(int clusterCount)
        {
            var clusters = await _classificationClusteringService.KMeansClusteringAsync(_dataTable, _selectedColumns.ToArray(), clusterCount);
            AnalysisResult = string.Join(", ", clusters);
        }

        private async Task PerformRegressionAnalysisAsync(string dependentVariable, float[] newValues)
        {
            var prediction = await _regressionAnalysisService.PredictAsync(_dataTable, dependentVariable, _selectedColumns.ToArray(), newValues);
            AnalysisResult = $"Prediction: {prediction}";
        }

        private async Task CalculateCorrelationAsync(string column1, string column2)
        {
            var correlation = await Task.Run(() => CorrelationAnalysis.CalculateCorrelation(_dataTable, column1, column2));
            AnalysisResult = $"Correlation: {correlation}";
        }

        private async Task CalculateMeanAsync(string columnName)
        {
            var mean = await Task.Run(() => _statisticalAnalysisService.CalculateMean(_dataTable, columnName));
            AnalysisResult = $"Mean: {mean}";
        }

        private async Task CalculateMedianAsync(string columnName)
        {
            var median = await Task.Run(() => _statisticalAnalysisService.CalculateMedian(_dataTable, columnName));
            AnalysisResult = $"Median: {median}";
        }

        private async Task CalculateStandardDeviationAsync(string columnName)
        {
            var stdDev = await Task.Run(() => _statisticalAnalysisService.CalculateStandardDeviation(_dataTable, columnName));
            AnalysisResult = $"Standard Deviation: {stdDev}";
        }

        private async Task ForecastTimeSeriesAsync(string columnName, int periods)
        {
            var forecast = await _timeSeriesAnalysisService.Forecast(_dataTable, columnName, periods);
            AnalysisResult = $"Forecast: {string.Join(", ", forecast)}";
        }

        private async Task DetectSeasonalPatternsAsync(string columnName)
        {
            var patterns = await _timeSeriesAnalysisService.SeasonalCyclicPatterns(_dataTable, columnName);
            AnalysisResult = $"Seasonal Patterns: {string.Join(", ", patterns)}";
        }
    }
}
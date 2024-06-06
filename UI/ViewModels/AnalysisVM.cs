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

namespace ThesisProjectARM.UI.ViewModels
{
    internal class AnalysisVM : INotifyPropertyChanged
    {
        private readonly IClassificationClustering classificationClusteringService;
        private readonly IRegressionAnalysis regressionAnalysisService;
        
        public ICommand PerformKMeansClusteringCommand { get; }
        public ICommand PerformRegressionAnalysisCommand { get; }
        public ICommand CalculateCorrelationCommand { get; }
        public ICommand CalculateMeanCommand { get; }
        public ICommand CalculateMedianCommand { get; }
        public ICommand CalculateStandardDeviationCommand { get; }
        public ICommand ForecastTimeSeriesCommand { get; }
        public ICommand DetectSeasonalPatternsCommand { get; }

        private DataTable selectedData;
        public DataTable SelectedData
        {
            get => selectedData;
            set
            {
                selectedData = value;
                OnPropertyChanged();
                UpdateColumnNames();
            }
        }

        private ObservableCollection<string> columnNames;
        public ObservableCollection<string> ColumnNames
        {
            get => columnNames;
            set
            {
                columnNames = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> selectedColumns;
        public ObservableCollection<string> SelectedColumns
        {
            get => selectedColumns;
            set
            {
                selectedColumns = value;
                OnPropertyChanged();
            }
        }

        public AnalysisVM(IClassificationClustering classificationClusteringService, IRegressionAnalysis regressionAnalysisService)
        {
            this.classificationClusteringService = classificationClusteringService;
            this.regressionAnalysisService = regressionAnalysisService;
            ColumnNames = new ObservableCollection<string>();
            SelectedColumns = new ObservableCollection<string>();
        }

        private void UpdateColumnNames()
        {
            ColumnNames.Clear();
            if (SelectedData != null)
            {
                foreach (DataColumn column in SelectedData.Columns)
                {
                    ColumnNames.Add(column.ColumnName);
                }
            }
        }

        public async Task PerformKMeansClusteringAsync(int clusterCount)
        {
            var clusters = await classificationClusteringService.KMeansClusteringAsync(SelectedData, SelectedColumns.ToArray(), clusterCount);
            // Отобразите результаты кластеризации
        }

        public async Task PerformRegressionAnalysisAsync(string dependentVariable, float[] newValues)
        {
            var independentVariables = SelectedColumns.Where(c => c != dependentVariable).ToArray();
            var prediction = await regressionAnalysisService.PredictAsync(SelectedData, dependentVariable, independentVariables, newValues);
            // Отобразите результаты предсказания
        }

        public async Task<double> CalculateCorrelationAsync(string column1, string column2)
        {
            return await Task.Run(() => CorrelationAnalysis.CalculateCorrelation(SelectedData, column1, column2));
        }

        public async Task<double> CalculateMeanAsync(string columnName)
        {
            return await Task.Run(() => StatisticalAnalysis.CalculateMean(SelectedData, columnName));
        }

        public async Task<double> CalculateMedianAsync(string columnName)
        {
            return await Task.Run(() => StatisticalAnalysis.ClaculateMedian(SelectedData, columnName));
        }

        public async Task<double> CalculateStandardDeviationAsync(string columnName)
        {
            return await Task.Run(() => StatisticalAnalysis.CalculateStandardDeviation(SelectedData, columnName));
        }

        public async Task<double[]> ForecastTimeSeriesAsync(string columnName, int periods)
        {
            return await Task.Run(() => TimeSeriesAnalysis.Forecast(SelectedData, columnName, periods));
        }

        public async Task<(double[] seasonal, double[] trend)> DetectSeasonalPatternsAsync(string columnName)
        {
            return await Task.Run(() => TimeSeriesAnalysis.SeasonalCyclicPatterns(SelectedData, columnName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public AnalysisVM(IClassificationClustering classificationClusteringService, IRegressionAnalysis regressionAnalysisService)
        {
            // Инициализация сервисов и коллекций

            PerformKMeansClusteringCommand = new RelayCommand(async () => await PerformKMeansClusteringAsync(3));
            PerformRegressionAnalysisCommand = new RelayCommand(async () =>
            {
                var dependentVariable = SelectedColumns.FirstOrDefault();
                if (dependentVariable != null)
                {
                    float[] newValues = new float[SelectedColumns.Count - 1];
                    await PerformRegressionAnalysisAsync(dependentVariable, newValues);
                }
            });
            CalculateCorrelationCommand = new RelayCommand(async () =>
            {
                if (SelectedColumns.Count >= 2)
                {
                    await CalculateCorrelationAsync(SelectedColumns[0], SelectedColumns[1]);
                }
            });
            CalculateMeanCommand = new RelayCommand(async () =>
            {
                if (SelectedColumns.Count > 0)
                {
                    await CalculateMeanAsync(SelectedColumns[0]);
                }
            });
            CalculateMedianCommand = new RelayCommand(async () =>
            {
                if (SelectedColumns.Count > 0)
                {
                    await CalculateMedianAsync(SelectedColumns[0]);
                }
            });
            CalculateStandardDeviationCommand = new RelayCommand(async () =>
            {
                if (SelectedColumns.Count > 0)
                {
                    await CalculateStandardDeviationAsync(SelectedColumns[0]);
                }
            });
            ForecastTimeSeriesCommand = new RelayCommand(async () =>
            {
                if (SelectedColumns.Count > 0)
                {
                    await ForecastTimeSeriesAsync(SelectedColumns[0], 5);
                }
            });
            DetectSeasonalPatternsCommand = new RelayCommand(async () =>
            {
                if (SelectedColumns.Count > 0)
                {
                    await DetectSeasonalPatternsAsync(SelectedColumns[0]);
                }
            });
        }
    }
}
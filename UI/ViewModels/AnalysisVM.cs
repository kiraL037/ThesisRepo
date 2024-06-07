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
        public DataTable SelectedData { get; set; }

        public AnalysisVM(IClassificationClustering classificationClusteringService, IRegressionAnalysis regressionAnalysisService)
        {
            this.classificationClusteringService = classificationClusteringService;
            this.regressionAnalysisService = regressionAnalysisService;
        }

        public void LoadSelectedData(DataTable dataTable)
        {
            SelectedData = dataTable;
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
            return await Task.Run(() => CorrelationAnalysis.CalculateCorrelation(selectedData, column1, column2));
        }

        public async Task<double> CalculateMeanAsync(string columnName)
        {
            return await Task.Run(() => StatisticalAnalysis.CalculateMean(selectedData, columnName));
        }

        public async Task<double> CalculateMedianAsync(string columnName)
        {
            return await Task.Run(() => StatisticalAnalysis.CalculateMedian(selectedData, columnName));
        }

        public async Task<double> CalculateStandardDeviationAsync(string columnName)
        {
            return await Task.Run(() => StatisticalAnalysis.CalculateStandardDeviation(selectedData, columnName));
        }

        public async Task<double[]> ForecastTimeSeriesAsync(string columnName, int periods)
        {
            return await Task.Run(() => TimeSeriesAnalysis.Forecast(selectedData, columnName, periods));
        }

        public async Task<(double[] seasonal, double[] trend)> DetectSeasonalPatternsAsync(string columnName)
        {
            return await Task.Run(() => TimeSeriesAnalysis.SeasonalCyclicPatterns(selectedData, columnName));
        }
    }
}
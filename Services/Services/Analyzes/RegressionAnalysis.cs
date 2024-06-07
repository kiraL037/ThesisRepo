using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ThesisProjectARM.Core.Interfaces;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace ThesisProjectARM.Services.Services.Analyzes
{
    public class RegressionAnalysis : IRegressionAnalysis
    {
        private readonly MLContext mlContext = new MLContext();

        public async Task<double> PredictAsync(DataTable dataTable, string dependentVariable, string[] independentVariables, float[] newValues)
        {
            return await Task.Run(() =>
            {
                var data = ConvertDataTableToIDataView(dataTable, dependentVariable, independentVariables);
                var model = TrainModel(data, dependentVariable, independentVariables);
                var predictionEngine = mlContext.Model.CreatePredictionEngine<InputData, Prediction>(model);
                var input = new InputData { Features = newValues };
                var prediction = predictionEngine.Predict(input);
                return prediction.Score;
            });
        }

        private IDataView ConvertDataTableToIDataView(DataTable dataTable, string dependentVariable, string[] independentVariables)
        {
            var data = dataTable.AsEnumerable().Select(row => new InputData
            {
                Label = Convert.ToSingle(row[dependentVariable]),
                Features = independentVariables.Select(col => Convert.ToSingle(row[col])).ToArray()
            }).ToList();

            return mlContext.Data.LoadFromEnumerable(data);
        }

        private ITransformer TrainModel(IDataView data, string dependentVariable, string[] independentVariables)
        {
            var pipeline = mlContext.Transforms.Concatenate("Features", independentVariables)
                .Append(mlContext.Regression.Trainers.Sdca(labelColumnName: "Label", maximumNumberOfIterations: 100));
            return pipeline.Fit(data);
        }
    }

    public class InputData
    {
        public float Label { get; set; }
        public float[] Features { get; set; }
    }

    public class Prediction
    {
        public float Score { get; set; }
    }
}
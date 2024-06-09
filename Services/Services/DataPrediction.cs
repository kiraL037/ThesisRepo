using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Services.Services
{
    public class DataPrediction
    {
        private MLContext mlContext = new MLContext();

        public void TrainAndPredict(DataTable dataTable)
        {
            var data = ConvertDataTableToIDataView(dataTable);
            var model = TrainModel(data);

            var predictions = Predict(model, data);
            DisplayPredictions(predictions);
        }

        private ITransformer TrainModel(IDataView data)
        {
            var pipeline = mlContext.Transforms.Concatenate("Features", new[]
            {"Feature1","Feature2"}).Append(mlContext.Regression.Trainers.Sdca
            (labelColumnName: "Label", maximumNumberOfIterations: 100));
            return pipeline.Fit(data);
        }

        private IEnumerable<Prediction> Predict(ITransformer model, IDataView data)
        {
            var predictionEngine = mlContext.Model.CreatePredictionEngine<InputData, Prediction>(model);
            var inputData = mlContext.Data.CreateEnumerable<InputData>(data, reuseRowObject: false);
            return inputData.Select(input => predictionEngine.Predict(input));
        }

        private void DisplayPredictions(IEnumerable<Prediction> predictions)
        {

        }

        private IDataView ConvertDataTableToIDataView(DataTable dataTable)
        {
            var data = new List<InputData>();
            foreach (DataRow row in dataTable.Rows)
            {
                data.Add(new InputData
                {
                    Feature1 = Convert.ToSingle(row["Feature1"]),
                    Feature2 = Convert.ToSingle(row["Feature2"]),
                    Label = Convert.ToSingle(row["Label"])
                });
            }
            return mlContext.Data.LoadFromEnumerable(data);
        }

        public class InputData
        {
            public float Feature1 { get; set; }
            public float Feature2 { get; set; }
            public float Label { get; set; }
        }

        public class Prediction
        {
            public float Score { get; set; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisProjectARM.Core.Interfaces;

namespace ThesisProjectARM.Services.Services.Analyzes
{
    public class ClassificationClustering : IClassificationClustering
    {
        public async Task<int[]> KMeansClusteringAsync(DataTable dataTable, string[] columnNames, int clusterCount)
        {
            return await Task.Run(() =>
            {
                var data = dataTable.AsEnumerable().Select(row => columnNames.Select(col => Convert.ToDouble(row[col])).ToArray()).ToArray();
                var kMeans = new KMeans(clusterCount);
                var clusters = kMeans.Learn(data);
                return clusters.Decide(data);
            });
        }

        public async Task<string> ClassifyAsync(DataTable dataTable, string[] columnNames, string targetColumnName, double[] newObject)
        {
            return await Task.Run(() =>
            {
                var data = dataTable.AsEnumerable().Select(row => columnNames.Select(col => Convert.ToDouble(row[col])).ToArray()).ToArray();
                var labels = dataTable.AsEnumerable().Select(row => Convert.ToInt32(row[targetColumnName])).ToArray();

                var teacher = new MulticlassSupportVectorLearning<Gaussian>()
                {
                    Learner = (param) => new SequentialMinimalOptimization<Gaussian>()
                    {
                        Complexity = 100
                    }
                };

                var svm = teacher.Learn(data, labels);

                var predicted = svm.Decide(newObject);
                return predicted.ToString();
            });
        }
    }
}
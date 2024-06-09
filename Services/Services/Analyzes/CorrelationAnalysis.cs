using System;
using System.Data;
using System.Linq;

namespace Services.Services.Analyzes
{
    public class CorrelationAnalysis
    {
        public static double CalculateCorrelation(DataTable dataTable, string column1, string column2)
        {
            var values1 = dataTable.AsEnumerable().Select(row => Convert.ToDouble(row[column1])).ToArray();
            var values2 = dataTable.AsEnumerable().Select(row => Convert.ToDouble(row[column2])).ToArray();
            double mean1 = values1.Average();
            double mean2 = values2.Average();
            double covariance = values1.Zip(values2, (x, y) => (x - mean1) * (y - mean2)).Sum() / values1.Length;
            double standardDeviation1 = Math.Sqrt(values1.Sum(x => Math.Pow(x - mean1, 2)) / values1.Length);
            double standardDeviation2 = Math.Sqrt(values2.Sum(x => Math.Pow(x - mean2, 2)) / values2.Length);
            return covariance / (standardDeviation1 * standardDeviation2);
        }
    }
}

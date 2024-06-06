using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisProjectARM.Services.Services.Analyzes
{
    public class StatisticalAnalysis : IStatisticalAnalysis
    {
        public double CalculateMean(DataTable data, string columnName)
        {
            var columnData = data.AsEnumerable().Select(row => Convert.ToDouble(row[columnName]));
            return columnData.Average();
        }

        public double CalculateMedian(DataTable data, string columnName)
        {
            var columnData = data.AsEnumerable().Select(row => Convert.ToDouble(row[columnName])).OrderBy(n => n).ToArray();
            int count = columnData.Length;
            if (count % 2 == 0)
            {
                return (columnData[count / 2 - 1] + columnData[count / 2]) / 2.0;
            }
            return columnData[count / 2];
        }

        public double CalculateStandardDeviation(DataTable data, string columnName)
        {
            var columnData = data.AsEnumerable().Select(row => Convert.ToDouble(row[columnName]));
            double mean = columnData.Average();
            double sumOfSquaresOfDifferences = columnData.Select(val => (val - mean) * (val - mean)).Sum();
            return Math.Sqrt(sumOfSquaresOfDifferences / columnData.Count());
        }
    }
}
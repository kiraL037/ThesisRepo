using System.Data;

namespace Core.Interfaces
{
    public interface IStatisticalAnalysis
    {
        double CalculateMean(DataTable data, string columnName);
        double CalculateMedian(DataTable data, string columnName);
        double CalculateStandardDeviation(DataTable data, string columnName);
    }
}
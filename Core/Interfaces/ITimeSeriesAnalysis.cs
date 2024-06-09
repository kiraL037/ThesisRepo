using System.Data;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ITimeSeriesAnalysis
    {
        Task<double[]> Forecast(DataTable data, string columnName, int periods);
        Task<(double[] seasonal, double[] trend)> SeasonalCyclicPatterns(DataTable data, string columnName);
    }
}

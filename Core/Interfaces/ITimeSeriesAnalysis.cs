using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ITimeSeriesAnalysis
    {
        double[] Forecast(DataTable data, string columnName, int periods);
        (double[] seasonal, double[] trend) SeasonalCyclicPatterns(DataTable data, string columnName);
    }
}

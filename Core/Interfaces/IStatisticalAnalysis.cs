using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IStatisticalAnalysis
    {
        double CalculateMean(DataTable data, string columnName);
        double CalculateMedian(DataTable data, string columnName);
        double CalculateStandardDeviation(DataTable data, string columnName);
    }
}
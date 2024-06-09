using System.Data;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRegressionAnalysis
    {
        Task<double> PredictAsync(DataTable data, string dependentVariable, string[] independentVariables, float[] newValues);
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisProjectARM.Core.Interfaces
{
    public interface IRegressionAnalysis
    {
        Task<object> PredictAsync(DataTable data, string dependentVariable, string[] independentVariables, float[] newValues);
    }
}
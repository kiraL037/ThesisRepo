
using System.Data;

namespace Core.Interfaces
{
    public interface IDataAnalyzer
    {
        DataTable AnalyzeData(DataTable data, string analysisType);
    }
}

using System.Data;
using OxyPlot;

namespace Core.Interfaces
{
    public interface IDataVisualizer
    {
        PlotModel PlotData(DataTable data, string columnName);
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using OxyPlot;
using System.Threading.Tasks;

namespace ThesisProjectARM.Core.Interfaces
{
    public interface IDataVisualizer
    {
        PlotModel PlotData(DataTable data, string columnName);
    }
}

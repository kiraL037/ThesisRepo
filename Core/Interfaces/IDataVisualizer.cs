using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisProjectARM.Core.Interfaces
{
    public interface IDataVisualizer
    {
        void PlotData(DataTable data, string columnName);
    }
}

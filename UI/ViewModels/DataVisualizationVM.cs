using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisProjectARM.UI.ViewModels
{
    public class DataVisualizationVM
    {
        private readonly IDataVisualizer dataVisualizerService;

        public DataVisualizationVM(IDataVisualizer dataVisualizerService)
        {
            this.dataVisualizerService = dataVisualizerService;
        }

        public void VisualizeData(DataTable data, string columnName)
        {
            dataVisualizerService.PlotData(data, columnName);
        }
    }
}

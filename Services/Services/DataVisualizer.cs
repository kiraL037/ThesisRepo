using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Series;
using ThesisProjectARM.Core.Interfaces;

namespace ThesisProjectARM.Services.Services
{
    public class DataVisualizer : IDataVisualizer
    {
        public void PlotData(DataTable data, string columnName)
        {
            var plotModel = new PlotModel { Title = "Data Plot" };

            var series = new LineSeries();
            foreach (DataRow row in data.Rows)
            {
                series.Points.Add(new DataPoint(Convert.ToDouble(row["Index"]), Convert.ToDouble(row[columnName])));
            }

            plotModel.Series.Add(series);

            var plotView = new PlotView { Model = plotModel };
            var window = new Window
            {
                Content = plotView,
                Width = 800,
                Height = 600,
                Title = "Data Visualization"
            };
            window.ShowDialog();
        }
    }
}
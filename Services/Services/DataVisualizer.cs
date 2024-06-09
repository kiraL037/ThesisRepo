using System;
using System.Data;
using OxyPlot;
using OxyPlot.Series;
using Core.Interfaces;

namespace Services.Services
{
    public class DataVisualizer : IDataVisualizer
    {
        public PlotModel PlotData(DataTable data, string columnName)
        {
            var plotModel = new PlotModel { Title = "Data Plot" };

            var series = new LineSeries();
            foreach (DataRow row in data.Rows)
            {
                series.Points.Add(new DataPoint(Convert.ToDouble(row["Index"]), Convert.ToDouble(row[columnName])));
            }

            plotModel.Series.Add(series);

            return plotModel;
        }
    }
}
﻿using Core.Interfaces;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services.Analyzes
{
    public class TimeSeriesAnalysis : ITimeSeriesAnalysis
    {
        public async Task<double[]> Forecast(DataTable data, string columnName, int periods)
        {
            // Пример простой реализации на основе скользящего среднего
            var columnData = data.AsEnumerable().Select(row => Convert.ToDouble(row[columnName])).ToArray();
            double[] forecast = new double[periods];
            double sum = columnData.Sum();
            double mean = sum / columnData.Length;
            for (int i = 0; i < periods; i++)
            {
                forecast[i] = mean;
            }
            return await Task.FromResult(forecast);
        }

        public async Task<(double[] seasonal, double[] trend)> SeasonalCyclicPatterns(DataTable data, string columnName)
        {
            // Пример простой реализации для обнаружения сезонных циклов
            var columnData = data.AsEnumerable().Select(row => Convert.ToDouble(row[columnName])).ToArray();
            double[] seasonal = new double[columnData.Length];
            double[] trend = new double[columnData.Length];

            // Пример простой реализации (фиктивные данные)
            for (int i = 0; i < columnData.Length; i++)
            {
                seasonal[i] = columnData[i] % 2 == 0 ? 1 : -1;
                trend[i] = columnData[i];
            }
            return await Task.FromResult((seasonal, trend));
        }
    }
}
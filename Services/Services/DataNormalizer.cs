using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ThesisProjectARM.Core.Interfaces;

namespace ThesisProjectARM.Services.Services
{
    public class DataNormalizer : IDataNormalizer
    {
        // Реализация нормализации данных
        public DataTable Normalize(DataTable data)
        {
            try
            {
                Parallel.ForEach(data.Columns.Cast<DataColumn>(), (column) =>
                {
                    if (column.DataType == typeof(double) || column.DataType == typeof(int))
                    {
                        var values = data.AsEnumerable()
                            .Select(row => Convert.ToDouble(row[column]))
                            .ToArray();

                        double min = values.Min();
                        double max = values.Max();

                        Parallel.ForEach(data.Rows.Cast<DataRow>(), (row) =>
                        {
                            row[column] = (Convert.ToDouble(row[column]) - min) / (max - min);
                        });
                    }
                });
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in NormalizeData: {ex.Message}");
                MessageBox.Show($"An error occurred during data normalization: {ex.Message}");
                return null;
            }
        }
    }
}

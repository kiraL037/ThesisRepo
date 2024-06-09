using System;
using System.Data;
using System.Linq;
using Core.Interfaces;

namespace Services.Services
{
    public class FillMissingValues : IFillMissingValues
    {
        public DataTable Fill(DataTable data)
        {
            // Реализация заполнения пропущенных значений
            foreach (DataColumn column in data.Columns)
            {
                if (column.DataType == typeof(double) || column.DataType == typeof(int))
                {
                    var values = data.AsEnumerable()
                        .Where(row => !row.IsNull(column))
                        .Select(row => Convert.ToDouble(row[column]))
                        .ToList();

                    double mean = values.Average();

                    foreach (DataRow row in data.Rows)
                    {
                        if (row.IsNull(column))
                        {
                            row[column] = mean;
                        }
                    }
                }
            }
            return data;
        }
    }
}

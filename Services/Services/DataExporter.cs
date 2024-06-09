using System.Data;
using System.IO;
using Core.Interfaces;

namespace Services.Services
{
    public class DataExporter : IDataExporter
    {
        public void ExportData(DataTable data, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (DataColumn column in data.Columns)
                {
                    writer.Write(column.ColumnName + ",");
                }
                writer.WriteLine();

                foreach (DataRow row in data.Rows)
                {
                    foreach (DataColumn column in data.Columns)
                    {
                        writer.Write(row[column] + ",");
                    }
                    writer.WriteLine();
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisProjectARM.Services.Services;
using ThesisProjectARM.Core.Interfaces;
using System.Data;
using System.IO;

namespace ThesisProjectARM.Services.Services
{
    public class DataService : IDataService
    {
        public DataTable LoadData(string filePath)
        {
            try
            {
                var dataTable = new DataTable();
                using (var reader = new StreamReader(filePath))
                {
                    var headers = reader.ReadLine().Split(',');
                    foreach (var header in headers)
                    {
                        dataTable.Columns.Add(header);
                    }

                    while (!reader.EndOfStream)
                    {
                        var rows = reader.ReadLine().Split(',');
                        var dataRow = dataTable.NewRow();
                        for (var i = 0; i < headers.Length; i++)
                        {
                            dataRow[i] = rows[i];
                        }
                        dataTable.Rows.Add(dataRow);
                    }
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading data", ex);
            }
        }

        public void SaveData(DataTable data, string filePath)
        {
            try
            {
                using (var writer = new StreamWriter(filePath))
                {
                    var columnNames = data.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
                    writer.WriteLine(string.Join(",", columnNames));

                    foreach (DataRow row in data.Rows)
                    {
                        var fields = row.ItemArray.Select(field => field.ToString());
                        writer.WriteLine(string.Join(",", fields));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving data", ex);
            }
        }
    }
}
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisProjectARM.Core.Interfaces;
using ThesisProjectARM.Core.Models;


namespace ThesisProjectARM.Services.Services
{
    internal class CSVDatainfo : IGetData
    {
        private string filePath;

        public CSVDataInfo(string filePath)
        {
            this.filePath = filePath;
        }

        public async Task<InfoMetaData> GetMetaDataAsync()
        {
            var metadata = new InfoMetaData
            {
                DataNameInfo = Path.GetFileName(filePath),
                Columns = new List<ColumnMetadata>()
            };

            using (StreamReader reader = new StreamReader(filePath))
            {
                string headerLine = await reader.ReadLineAsync();
                string[] headers = headerLine.Split(',');

                foreach (string header in headers)
                {
                    metadata.Columns.Add(new ColumnMetadata
                    {
                        ColumnName = header,
                        DataType = "stryng"
                    });
                }
            }
            return metadata;
        }
        public async Task<DataTable> GetDataAsync()
        {
            DataTable dataTable = new DataTable();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string headerLine = await reader.ReadLineAsync();
                string[] headers = headerLine.Split(',');

                foreach (string header in headers)
                {
                    dataTable.Columns.Add(new DataColumn(header));
                }

                while (!reader.EndOfStream)
                {
                    string line = await reader.ReadLineAsync();
                    string[] fields = line.Split(',');
                    dataTable.Rows.Add(fields);
                }
            }
            return dataTable;
        }
    }
}

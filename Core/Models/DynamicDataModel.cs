using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisProjectARM.Core.Models
{
    public class DynamicDataModel
    {
        public int Id { get; set; }
        public Dictionary<string, object> Data { get; set; }

        public DynamicDataModel()
        {
            Data = new Dictionary<string, object>();
        }
        public DataRow ToDataRow(DataTable dataTable)
        {
            var row = dataTable.NewRow();
            foreach (var key in Data.Keys)
            {
                if (dataTable.Columns.Contains(key))
                {
                    row[key] = Data[key];
                }
            }
            return row;
        }
    }
}
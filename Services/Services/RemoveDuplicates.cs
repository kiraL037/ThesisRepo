using System.Data;
using System.Linq;
using Core.Interfaces;

namespace Services.Services
{
    public class RemoveDuplicates : IRemoveDuplicates
    {
        public DataTable Remove(DataTable data)
        {
            var uniqueRows = data.AsEnumerable().Distinct(DataRowComparer.Default);
            DataTable newDataTable = data.Clone();
            foreach (var row in uniqueRows)
            {
                newDataTable.ImportRow(row);
            }
            return newDataTable;
        }
    }
}

using System.Data;

namespace Core.Interfaces
{
    public interface IDataExporter
    {
        void ExportData(DataTable data, string filePath);
    }
}
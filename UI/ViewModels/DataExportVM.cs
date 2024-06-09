using System.Data;
using Core.Interfaces;

namespace UI.ViewModels
{
    public class DataExportVM
    {
        private readonly IDataExporter dataExporterService;

        public DataExportVM(IDataExporter dataExporterService)
        {
            this.dataExporterService = dataExporterService;
        }

        public void ExportData(DataTable data, string filePath)
        {
            dataExporterService.ExportData(data, filePath);
        }
    }
}
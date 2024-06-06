using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisProjectARM.Core.Interfaces;

namespace ThesisProjectARM.UI.ViewModels
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
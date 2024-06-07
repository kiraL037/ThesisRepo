using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisProjectARM.Core.Interfaces;
using ThesisProjectARM.Core.Models;

namespace ThesisProjectARM.Services.Services
{
    public class DataManager
    {
        private IDataInfo dataInfo;

        public void SetDataInfo(IDataInfo dataInfo)
        {
            this.dataInfo = dataInfo;
        }

        public async Task<DataInfoMetaData> LoadMetaDataAsync()
        {
            return await dataInfo.GetMetaDataAsync();
        }

        public async Task<DataTable> LoadDataAsync()
        {
            return await dataInfo.GetDataAsync();
        }
    }
}

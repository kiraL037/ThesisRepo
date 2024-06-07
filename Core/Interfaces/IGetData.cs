using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisProjectARM.Core.Models;

namespace Core.Interfaces
{
    public interface IGetData
    {
        Task<InfoMetaData> GetMetaDataAsync();
        Task<DataTable> GetDataAsync();
    }
}

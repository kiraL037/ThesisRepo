using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisProjectARM.Core.Models;

namespace ThesisProjectARM.Core.Interfaces
{
    public interface IDataInfo
    {
        Task<ObservableCollection<DynamicDataModel>> LoadDataAsync(string tableName);
        Task InsertDataAsync(string tableName, DynamicDataModel model);
        Task UpdateDataAsync(string tableName, DynamicDataModel model, int id);
        Task DeleteDataAsync(string tableName, int id);
    }
}

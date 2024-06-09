using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces
{
    public interface IDataInfo
    {
        Task<ObservableCollection<DynamicDataModel>> LoadDataAsync(string tableName);
        Task InsertDataAsync(string tableName, DynamicDataModel model);
        Task UpdateDataAsync(string tableName, DynamicDataModel model, int id);
        Task DeleteDataAsync(string tableName, int id);
    }
}

using System.Data;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces
{
    public interface IGetData
    {
        Task<InfoMetaData> GetMetaDataAsync();
        Task<DataTable> GetDataAsync();
        Task SaveDataAsync(DataTable dataTable);
    }
}

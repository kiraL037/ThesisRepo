using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Interfaces
{
    public interface IDBCHService
    {
        string BuildConnectionString(ConnectionModel connection);
        Task<bool> TestConnectionAsync(string connectionString);
        Task<DataTable> LoadDataAsync(string connectionString, string tableName);
        Task<IEnumerable<string>> GetTablesAsync(string connectionString);
        Task<IEnumerable<string>> GetColumnsAsync(string connectionString, string tableName);
        Task InsertDataAsync(string tableName, DynamicDataModel data);
    }
}
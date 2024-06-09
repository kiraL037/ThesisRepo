using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisProjectARM.Core.Models;

namespace ThesisProjectARM.Core.Interfaces
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
using Core.Models;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Core.Interfaces
{
    public interface IDatabaseService
    {
        Task<bool> SetupDatabaseAsync(ConnectionModel connection);
        string BuildConnectionString(ConnectionModel connection);
        Task<bool> TestConnectionAsync(string connectionString);
        Task<bool> AdminUserExistsAsync(SqlConnection connection);
    }
}
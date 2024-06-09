using ThesisProjectARM.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ThesisProjectARM.Core.Interfaces
{
    public interface IDatabaseService
    {
        Task<bool> SetupDatabaseAsync(ConnectionModel connection);
        string BuildConnectionString(ConnectionModel connection);
        Task<bool> TestConnectionAsync(string connectionString);
        Task<bool> AdminUserExistsAsync(SqlConnection connection);
    }
}
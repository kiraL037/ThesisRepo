using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Core.Interfaces;
using Core.Models;
using NLog;

namespace Services.Services
{
    public class DBService : IDatabaseService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public async Task<bool> SetupDatabaseAsync(ConnectionModel connection)
        {
            return await ExecuteDatabaseCommandAsync(connection.ConnectionString, async (conn) =>
            {
                await CreateDatabaseIfNotExistsAsync(conn, connection.Database);
                await CreateUsersTableIfNotExistsAsync(conn);
            });
        }

        private async Task<bool> ExecuteDatabaseCommandAsync(string connectionString, Func<SqlConnection, Task> command)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    await command(conn);
                }
                return true;
            }
            catch (SqlException sqlEx)
            {
                Logger.Error(sqlEx, "Ошибка инициализации базы данных");
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка инициализации базы данных");
                return false;
            }
        }

        public string BuildConnectionString(ConnectionModel connection)
        {
            if (string.IsNullOrEmpty(connection.Username) && string.IsNullOrEmpty(connection.Password))
            {
                // Windows Authentication
                return $"Server={connection.Server};Database={connection.Database};Integrated Security=True;";
            }
            else
            {
                // SQL Server Authentication
                return $"Server={connection.Server};Database={connection.Database};User Id={connection.Username};Password={connection.Password};";
            }
        }

        private async Task CreateDatabaseIfNotExistsAsync(SqlConnection conn, string databaseName)
        {
            string createDbQuery = $"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{databaseName}') CREATE DATABASE [{databaseName}]";
            using (SqlCommand command = new SqlCommand(createDbQuery, conn))
            {
                await command.ExecuteNonQueryAsync();
            }
        }

        private async Task CreateUsersTableIfNotExistsAsync(SqlConnection conn)
        {
            string createTableQuery = @"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
                CREATE TABLE Users (
                    Id INT PRIMARY KEY IDENTITY,
                    Username NVARCHAR(50) NOT NULL,
                    PasswordHash NVARCHAR(256) NOT NULL,
                    Salt NVARCHAR(256) NOT NULL,
                    IsAdmin BIT NOT NULL
                )";
            using (SqlCommand command = new SqlCommand(createTableQuery, conn))
            {
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<bool> TestConnectionAsync(string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    return true;
                }
            }
            catch (SqlException sqlEx)
            {
                Logger.Error(sqlEx, "Ошибка тестирования строки подключения");
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка тестирования строки подключения");
                return false;
            }
        }

        public async Task<bool> AdminUserExistsAsync(SqlConnection connection)
        {
            string query = "SELECT COUNT(*) FROM Users WHERE IsAdmin = 1";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                int adminCount = (int)await command.ExecuteScalarAsync();
                return adminCount > 0;
            }
        }
    }
}

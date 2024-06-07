using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ThesisProjectARM.Core.Interfaces;
using ThesisProjectARM.Core.Models;
using NLog;

namespace ThesisProjectARM.Services.Services
{
    public class DBService : IDatabaseService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public async Task<bool> SetupDatabaseAsync(ConnectionModel connection)
        {
            try
            {
                var connectionString = BuildConnectionString(connection);
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    string createDbQuery = $"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{connection.Database}') CREATE DATABASE [{connection.Database}]";
                    using (SqlCommand command = new SqlCommand(createDbQuery, conn))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                    string useDatabaseQuery = "USE DB_THESIS";
                    using (SqlCommand command = new SqlCommand(useDatabaseQuery, conn))
                    {
                        await command.ExecuteNonQueryAsync();
                    }

                    // Дополнительная логика создания таблиц
                    string createTableQuery = @"
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
                    CREATE TABLE Users (
                        Id INT PRIMARY KEY IDENTITY,
                        Username NVARCHAR(50) NOT NULL,
                        PasswordHash NVARCHAR(64) NOT NULL,
                        IsAdmin BIT NOT NULL
                    )";
                    using (SqlCommand command = new SqlCommand(createTableQuery, conn))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }
                return true;
            }
            catch (SqlException sqlEx)
            {
                Logger.Error(sqlEx, "Ошибка инициализации базы данных");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка инициализации базы данных");
                throw;
            }
        }

        public string BuildConnectionString(ConnectionModel connection)
        {
            return $"Server={connection.Server};Database={connection.Database};User Id={connection.Username};Password={connection.Password};";
        }
    }
}

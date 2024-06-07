using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ThesisProjectARM.Core.Interfaces;
using NLog;
using static System.Net.Mime.MediaTypeNames;

namespace ThesisProjectARM.Services.Services
{
    public class DataEntryService : IDataEntryRepository
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

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

        public async Task InitializeDatabaseAsync(string connectionString)
        {
            try
            {
                string dbConnectionString = connectionString.Replace("master", "DB_THESIS");
                using (SqlConnection connection = new SqlConnection(dbConnectionString))
                {
                    await connection.OpenAsync();
                    if (!await AdminUserExistsAsync(connection))
                    {
                        OpenAdminCreationWindow(dbConnectionString);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Logger.Error(sqlEx, "SQL Ошибка инициализации базы данных");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка инициализации базы данных");
                throw;
            }
        }

        private async Task<bool> AdminUserExistsAsync(SqlConnection connection)
        {
            string query = "SELECT COUNT(*) FROM Users WHERE IsAdmin = 1";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                int adminCount = (int)await command.ExecuteScalarAsync();
                return adminCount > 0;
            }
        }

        private void OpenAdminCreationWindow(string connectionString)
        {
            var adminWindow = new AdminWindow(connectionString);
            if (adminWindow.ShowDialog() != true)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using ThesisProjectARM.Core.Interfaces;
using ThesisProjectARM.Core.Models;
using NLog;

namespace ThesisProjectARM.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
       
        public async Task<bool> AdminUserExistsAsync()
        {
            string connectionString = Settings.Default.ConnectionString.Replace("master", "DB_THESIS");
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string query = "SELECT COUNT(*) FROM Users WHERE IsAdmin = 1";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        int adminCount = (int)await command.ExecuteScalarAsync();
                        return adminCount > 0;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Logger.Error(sqlEx, "Ошибка проверки наличия администратора");
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка проверки наличия администратора");
                throw;
            }
        }
public async Task<User> GetUserByUsernameAsync(string username)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT Username, PasswordHash, Salt, IsAdmin FROM Users WHERE Username = @username";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new User
                            {
                                Username = reader.GetString(0),
                                PasswordHash = reader.GetString(1),
                                Salt = reader.GetString(2),
                                IsAdmin = reader.GetBoolean(3)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public async Task CreateUserAsync(User user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "INSERT INTO Users (Username, PasswordHash, Salt, IsAdmin) VALUES (@username, @passwordHash, @salt, @isAdmin)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", user.Username);
                    command.Parameters.AddWithValue("@passwordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("@salt", user.Salt);
                    command.Parameters.AddWithValue("@isAdmin", user.IsAdmin);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @username";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    return (int)await command.ExecuteScalarAsync() > 0;
                }
            }
        }
    }
}

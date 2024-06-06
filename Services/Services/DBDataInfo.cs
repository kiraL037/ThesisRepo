using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisProjectARM.Services.Services
{
    public class DBDataInfo : IDataService
    {
        private readonly string _connectionString;

        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<ObservableCollection<DynamicDataModel>> LoadDataAsync(string tableName)
        {
            var data = new ObservableCollection<DynamicDataModel>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"SELECT * FROM {tableName}";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    await connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var model = new DynamicDataModel();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                model.Data[reader.GetName(i)] = reader.GetValue(i);
                            }
                            data.Add(model);
                        }
                    }
                }
            }
            return data;
        }

        public async Task InsertDataAsync(string tableName, DynamicDataModel model)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var columns = string.Join(", ", model.Data.Keys);
                var parameters = string.Join(", ", model.Data.Keys.Select(key => "@" + key));
                string query = $"INSERT INTO {tableName} ({columns}) VALUES ({parameters})";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    foreach (var kvp in model.Data)
                    {
                        command.Parameters.AddWithValue("@" + kvp.Key, kvp.Value);
                    }
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateDataAsync(string tableName, DynamicDataModel model, int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var setClauses = string.Join(", ", model.Data.Keys.Select(key => $"{key} = @{key}"));
                string query = $"UPDATE {tableName} SET {setClauses} WHERE Id = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    foreach (var kvp in model.Data)
                    {
                        command.Parameters.AddWithValue("@" + kvp.Key, kvp.Value);
                    }
                    command.Parameters.AddWithValue("@id", id);
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteDataAsync(string tableName, int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = $"DELETE FROM {tableName} WHERE Id = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
}

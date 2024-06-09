using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisProjectARM.Services.Services;
using ThesisProjectARM.Core.Interfaces;
using System.Data;
using System.IO;
using ThesisProjectARM.Core.Models;

namespace ThesisProjectARM.Services.Services
{
    public class DBCHService : IDBCHService
    {

        public string BuildConnectionString(ConnectionModel connection)
        {
            if (connection.UseWindowsAuthentication)
            {
                return $"Server={connection.Server};Database={connection.Database};Integrated Security=True;";
            }
            else
            {
                return $"Server={connection.Server};Database={connection.Database};User Id={connection.Username};Password={connection.Password};";
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
            catch
            {
                return false;
            }
        }

        public async Task<DataTable> LoadDataAsync(string connectionString, string tableName)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string query = $"SELECT * FROM {tableName}";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            dataTable.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading data from database: {ex.Message}");
            }

            return dataTable;
        }

        public async Task<IEnumerable<string>> GetTablesAsync(string connectionString)
        {
            List<string> tables = new List<string>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    DataTable schema = connection.GetSchema("Tables");
                    foreach (DataRow row in schema.Rows)
                    {
                        tables.Add(row[2].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting tables from database: {ex.Message}");
            }

            return tables;
        }

        public async Task<IEnumerable<string>> GetColumnsAsync(string connectionString, string tableName)
        {
            List<string> columns = new List<string>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string query = $"SELECT * FROM {tableName} WHERE 1 = 0";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            DataTable schema = reader.GetSchemaTable();
                            foreach (DataRow row in schema.Rows)
                            {
                                columns.Add(row.Field<string>("ColumnName"));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting columns from database: {ex.Message}");
            }

            return columns;
        }

        public async Task InsertDataAsync(string tableName, DynamicDataModel data)
        {
            // Реализация вставки данных в базу данных
            throw new NotImplementedException();
        }
    }
}
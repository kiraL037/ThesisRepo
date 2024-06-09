using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Threading.Tasks;
using ThesisProjectARM.Services.Services;
using ThesisProjectARM.Core.Models;

namespace ThesisProjectARM.Tests.Tests
{
    [TestClass]
    public class DataServiceTests
    {
        [TestMethod]
        public async Task TestLoadData()
        {
            var service = new DBCHService();
            string connectionString = "your_connection_string";
            string tableName = "your_table_name";

            DataTable data = await service.LoadDataAsync(connectionString, tableName); // Исправлено
        }

        public async Task TestSaveData()
        {
            var service = new DBCHService();
            string connectionString = "your_connection_string";
            string tableName = "your_table_name";
            DynamicDataModel data = new DynamicDataModel();

            await service.InsertDataAsync(tableName, data); // Исправлено
        }
    }
}
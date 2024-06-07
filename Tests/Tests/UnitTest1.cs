using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using ThesisProjectARM.Services.Services;
using ThesisProjectARM.Data;

namespace ThesisProjectARM.Tests.Tests
{
    [TestClass]
    public class DataServiceTests
    {
        [TestMethod]
        public void LoadData_ShouldReturnDataTable()
        {
            // Arrange
            var dataService = new DataService();
            var filePath = "path_to_test_file.csv";

            // Act
            var result = dataService.LoadData(filePath);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DataTable));
        }

        [TestMethod]
        public void SaveData_ShouldSaveWithoutExceptions()
        {
            // Arrange
            var dataService = new DataService();
            var dataTable = new DataTable();
            var filePath = "path_to_save_file.csv";

            // Act & Assert
            dataService.SaveData(dataTable, filePath);
        }
    }
}
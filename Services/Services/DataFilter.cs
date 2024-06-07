using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ThesisProjectARM.Services.Services
{
    public class DataFilter
    {// Реализация фильтрации данных
        public DataTable Filter(DataTable data)
        {
            try
            {
                return data.AsEnumerable()
                       .Where(row => !row.ItemArray.Any(field => field == DBNull.Value || string.IsNullOrEmpty(field.ToString())))
                       .CopyToDataTable();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в фильтровании данных: {ex.Message}");
                MessageBox.Show($"Ошибка возникает во время фильтрования: {ex.Message}");
                return null;
            }
        }
    }
}
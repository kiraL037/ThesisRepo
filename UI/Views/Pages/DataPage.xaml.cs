using System.Data;
using System.Windows.Controls;

namespace UI.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для DataPage.xaml
    /// </summary>
    public partial class DataPage : Page
    {
        public DataPage()
        {
            InitializeComponent();
        }
        public void LoadData(DataTable dataTable)
        {
            dataGrid.ItemsSource = dataTable.DefaultView;
        }
    }
}

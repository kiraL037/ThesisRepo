using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Input;
using ThesisProjectARM.Core.Models;

namespace ThesisProjectARM.UI.ViewModels
{
    public class ManagerVM : ViewModelBase
    {
        private ObservableCollection<ManagerItem> _managerItems;
        public ObservableCollection<ManagerItem> ManagerItems
        {
            get { return _managerItems; }
            set
            {
                _managerItems = value;
                OnPropertyChanged(nameof(ManagerItems));
            }
        }

        private ICommand _loadItemsCommand;
        public ICommand LoadItemsCommand
        {
            get
            {
                if (_loadItemsCommand == null)
                {
                    _loadItemsCommand = new RelayCommand(async param => await LoadItemsAsync(), param => CanLoadItems());
                }
                return _loadItemsCommand;
            }
        }

        private bool CanLoadItems()
        {
            // Logic to enable or disable the command
            return true;
        }

        private async Task LoadItemsAsync()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ThesisDB"].ConnectionString;
            string query = "SELECT Name, Description FROM Users";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();

                var items = new ObservableCollection<DynamicDataModel>();
                while (await reader.ReadAsync())
                {
                    var model = new DynamicDataModel();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        model.Data[reader.GetName(i)] = reader.GetValue(i);
                    }
                    items.Add(model);
                }
                ManagerItems = items;
            }
        }

        public ManagerVM()
        {
            ManagerItems = new ObservableCollection<DynamicDataModel>();
        }
    }

    public class ManagerItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
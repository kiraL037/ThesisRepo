using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ThesisProjectARM.Core.Models;
using ThesisProjectARM.Core.Interfaces;

namespace ThesisProjectARM.UI.ViewModels
{
    public class ManagerVM : ViewModelBase
    {
        private ObservableCollection<User> _items;
        public ObservableCollection<User> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        private string _databaseName;
        public string DatabaseName
        {
            get { return _databaseName; }
            set
            {
                if (SetProperty(ref _databaseName, value))
                {
                    LoadDataAsync();
                }
            }
        }

        public ICommand DeleteUserCommand { get; }
        public ICommand ChangePermissionsCommand { get; }

        public ManagerVM()
        {
            Items = new ObservableCollection<User>();

            DeleteUserCommand = new RelayCommand(DeleteUser, CanExecuteDeleteOrChange);
            ChangePermissionsCommand = new RelayCommand(ChangePermissions, CanExecuteDeleteOrChange);
        }

        private string GetConnectionString()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ThesisDB"].ConnectionString;
            var builder = new SqlConnectionStringBuilder(connectionString)
            {
                InitialCatalog = _databaseName
            };
            return builder.ToString();
        }

        private async void LoadDataAsync()
        {
            if (string.IsNullOrEmpty(_databaseName))
                return;

            string connectionString = GetConnectionString();
            string query = "SELECT Id, Username, IsAdmin FROM Users";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                await connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();

                var items = new ObservableCollection<User>();
                while (await reader.ReadAsync())
                {
                    var user = new User
                    {
                        Id = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        IsAdmin = reader.GetBoolean(2)
                    };
                    items.Add(user);
                }
                Items = items;
            }
        }

        private bool CanExecuteDeleteOrChange(object parameter)
        {
            return parameter is User;
        }

        private async void DeleteUser(object parameter)
        {
            if (parameter is User user)
            {
                string connectionString = GetConnectionString();
                string query = "DELETE FROM Users WHERE Id = @Id";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", user.Id);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }

                Items.Remove(user);
            }
        }

        private async void ChangePermissions(object parameter)
        {
            if (parameter is User user)
            {
                bool newRole = !user.IsAdmin;

                string connectionString = GetConnectionString();
                string query = "UPDATE Users SET IsAdmin = @IsAdmin WHERE Id = @Id";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@IsAdmin", newRole);
                    command.Parameters.AddWithValue("@Id", user.Id);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }

                user.IsAdmin = newRole;
                OnPropertyChanged(nameof(Items));
            }
        }
    }
}
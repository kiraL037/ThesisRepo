using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ThesisProjectARM.UI.Views.Windows;
using ThesisProjectARM.Core.Models;
using ThesisProjectARM.Core.Interfaces;
using System.Configuration;
using UI.Properties;

namespace ThesisProjectARM.UI.ViewModels
{
    public class FirstSetupWindowVM : INotifyPropertyChanged
    {
        private readonly IDatabaseService _databaseService;
        private string _server;
        private string _database;
        private string _username;
        private string _password;
        private string _connectionString;
        private bool _isSetupComplete;
        private int _authType;

        public FirstSetupWindowVM(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            AuthType = 0; // Default to Windows Authentication
        }

        public string Server
        {
            get => _server;
            set
            {
                _server = value;
                OnPropertyChanged();
            }
        }

        public string Database
        {
            get => _database;
            set
            {
                _database = value;
                OnPropertyChanged();
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string ConnectionString
        {
            get => _connectionString;
            set
            {
                _connectionString = value;
                OnPropertyChanged();
            }
        }

        public bool IsSetupComplete
        {
            get => _isSetupComplete;
            set
            {
                _isSetupComplete = value;
                OnPropertyChanged();
            }
        }

        public int AuthType
        {
            get => _authType;
            set
            {
                _authType = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsSqlAuthSelected));
            }
        }

        public bool IsSqlAuthSelected => AuthType == 1;

        public ICommand SetupCommand => new RelayCommand(async _ => await SetupDatabase());

        private async Task SetupDatabase()
        {
            var connection = new ConnectionModel
            {
                Server = Server,
                Database = Database,
                Username = IsSqlAuthSelected ? Username : null,
                Password = IsSqlAuthSelected ? Password : null,
                ConnectionString = _databaseService.BuildConnectionString(new ConnectionModel
                {
                    Server = Server,
                    Database = Database,
                    Username = IsSqlAuthSelected ? Username : null,
                    Password = IsSqlAuthSelected ? Password : null
                })
            };

            var result = await _databaseService.SetupDatabaseAsync(connection);

            if (result)
            {
                IsSetupComplete = true;
                MessageBox.Show("Database setup successfully.");
                SaveConnectionString(connection.ConnectionString);
                CloseWindow();
            }
            else
            {
                MessageBox.Show("Failed to setup database. Please check your connection details.");
            }
        }

        private void SaveConnectionString(string connectionString)
        {
            Settings.Default.ConnectionString = connectionString;
            Settings.Default.Save();
        }

        private void CloseWindow()
        {
            var window = Application.Current.Windows.OfType<FirstSetupWindow>().FirstOrDefault();
            window?.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
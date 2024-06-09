using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Core.Interfaces;
using Core.Models;
using Services.Services;

namespace UI.ViewModels
{
    public class SelectTableVM : ViewModelBase
    {
        private readonly IDBCHService _dbCHService;

        public string Server { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool UseWindowsAuthentication { get; set; }

        private bool _isUserEnabled;
        public bool IsUserEnabled
        {
            get => _isUserEnabled;
            set { _isUserEnabled = value; OnPropertyChanged(); }
        }

        private bool _isPasswordEnabled;
        public bool IsPasswordEnabled
        {
            get => _isPasswordEnabled;
            set { _isPasswordEnabled = value; OnPropertyChanged(); }
        }

        private ObservableCollection<string> _tables;
        public ObservableCollection<string> Tables
        {
            get => _tables;
            set { _tables = value; OnPropertyChanged(); }
        }

        private string _selectedTable;
        public string SelectedTable
        {
            get => _selectedTable;
            set { _selectedTable = value; OnPropertyChanged(); }
        }

        public ICommand ConnectCommand { get; }
        public ICommand LoadTablesCommand { get; }

        public SelectTableVM()
        {
            _dbCHService = new DBCHService();
            IsUserEnabled = true;
            IsPasswordEnabled = true;

            ConnectCommand = new RelayCommand(async (param) => await ConnectToDatabase());
            LoadTablesCommand = new RelayCommand(async (param) => await LoadTables());
        }

        private async Task ConnectToDatabase()
        {
            var connectionModel = new ConnectionModel
            {
                Server = Server,
                Database = Database,
                Username = User,
                Password = Password,
                UseWindowsAuthentication = UseWindowsAuthentication
            };

            string connectionString = _dbCHService.BuildConnectionString(connectionModel);
            bool isConnected = await _dbCHService.TestConnectionAsync(connectionString);

            if (isConnected)
            {
                MessageBox.Show("Successfully connected to the database.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                await LoadTables();
            }
            else
            {
                MessageBox.Show("Failed to connect to the database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadTables()
        {
            try
            {
                string connectionString = _dbCHService.BuildConnectionString(new ConnectionModel
                {
                    Server = Server,
                    Database = Database,
                    Username = User,
                    Password = Password,
                    UseWindowsAuthentication = UseWindowsAuthentication
                });

                Tables = new ObservableCollection<string>(await _dbCHService.GetTablesAsync(connectionString));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tables: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
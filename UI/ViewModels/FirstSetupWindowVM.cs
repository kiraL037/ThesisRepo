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

namespace ThesisProjectARM.UI.ViewModels
{
    public class FirstSetupViewModel : INotifyPropertyChanged
    {
        private readonly IDatabaseService _databaseService;
        private string _server;
        private string _database;
        private string _username;
        private string _password;
        private string _connectionString;
        private bool _isSetupComplete;

        public FirstSetupViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
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

        public ICommand SetupCommand => new RelayCommand(async _ => await SetupDatabase());

        private async Task SetupDatabase()
        {
            var connection = new ConnectionModel
            {
                Server = Server,
                Database = Database,
                Username = Username,
                Password = Password
            };

            try
            {
                IsSetupComplete = await _databaseService.SetupDatabaseAsync(connection);
                if (IsSetupComplete)
                {
                    ConnectionString = _databaseService.BuildConnectionString(connection);
                    MessageBox.Show("Setup successful. You can now start the application.", "Setup Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                    CloseWindow();
                    WelcomeWindow welcomeWindow = new WelcomeWindow();
                    welcomeWindow.Show();
                }
                else
                {
                    MessageBox.Show("Setup failed. Please check the server details and try again.", "Setup Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Setup failed. Error: {ex.Message}", "Setup Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                    break;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
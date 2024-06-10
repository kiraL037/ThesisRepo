using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using NLog;
using Core.Interfaces;
using UI.ViewModels;
using UI.Views.Windows;
using Services.Services;
using UI.Properties;
using Data.Repositories;
using Core.Models;
using System.Data;
using UI.Views.Pages;
using Services.Services.Analyzes;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace UI
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private IDatabaseService _dbService;
        private IUserRepository _userRepository;
        private FirstSetupWindowVM _firstSetupWindowVM;
        private AdminWindowVM _adminWindowVM;
        private WelcomeWindowVM _welcomeWindowVM;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Инициализация сервисов
            _dbService = new DBService();
            _userRepository = new UserRepository(Settings.Default.ConnectionString);
            var userService = new UserService(_userRepository);
            var securityMethods = new SecurityMethods();

            // Инициализация ViewModels
            _firstSetupWindowVM = new FirstSetupWindowVM(_dbService);
            _adminWindowVM = new AdminWindowVM(userService, securityMethods);
            _welcomeWindowVM = new WelcomeWindowVM();

            string connectionString = Settings.Default.ConnectionString;

            if (string.IsNullOrEmpty(connectionString) || !await _dbService.TestConnectionAsync(connectionString))
            {
                OpenFirstSetupWindow();
            }
            else
            {
                await InitializeDatabaseAsync(connectionString);
                ShowManagerOrWelcomeWindow();
            }
        }

        private void OpenFirstSetupWindow()
        {
            var firstSetupWindow = new FirstSetupWindow(_firstSetupWindowVM);
            firstSetupWindow.ShowDialog();
        }

        private async Task InitializeDatabaseAsync(string connectionString)
        {
            try
            {
                await _dbService.SetupDatabaseAsync(new ConnectionModel { ConnectionString = connectionString });
            }
            catch (SqlException sqlEx)
            {
                Logger.Error(sqlEx, "Ошибка инициализации SQL базы данных");
                MessageBox.Show($"Ошибка инициализации SQL базы данных: {sqlEx.Message}");
                Shutdown();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка инициализации базы данных");
                MessageBox.Show($"Ошибка инициализации базы данных: {ex.Message}");
                Settings.Default.ConnectionString = string.Empty;
                Settings.Default.Save();
                Shutdown();
            }
        }

        private async void ShowManagerOrWelcomeWindow()
        {
            bool adminExists = await _userRepository.AdminUserExistsAsync();

            if (adminExists)
            {
                var adminWindow = new AdminWindow(_adminWindowVM);
                adminWindow.ShowDialog();
            }
            else
            {
                var welcomeWindow = new WelcomeWindow(_welcomeWindowVM);
                welcomeWindow.ShowDialog();
            }
        }
    }
}
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using SimpleInjector;
using NLog;
using System.ComponentModel;
using ThesisProjectARM.Core.Interfaces;
using ThesisProjectARM.UI.ViewModels;
using ThesisProjectARM.UI.Views.Windows;
using ThesisProjectARM.Data;
using ThesisProjectARM.Services.Services;
using UI.Properties;

namespace UI
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Container _container;

        public App()
        {
            _container = new Container();
            ConfigureContainer();
        }

        private void ConfigureContainer()
        {
            _container.Register<IUserRepository, UserRepository>(Lifestyle.Singleton);
            _container.Register<IDatabaseService, DatabaseService>(Lifestyle.Singleton);
            _container.Register<IWindowService, WindowService>(Lifestyle.Singleton);
            _container.Register<ManagerVM>(Lifestyle.Singleton);
            _container.Register<RegistrationVM>(Lifestyle.Singleton);
            _container.Register<SelectTableVM>(Lifestyle.Singleton);
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            string connectionString = Settings.Default.ConnectionString;
            if (string.IsNullOrEmpty(connectionString) || !await TestConnectionStringAsync(connectionString))
            {
                OpenDatabaseConnectionWindow();
                connectionString = Settings.Default.ConnectionString;
            }
            await InitializeDatabaseAsync(connectionString);
            ShowWelcomeWindow();
        }

        private void OpenDatabaseConnectionWindow()
        {
            var databaseConnectionWindow = new SelectTableDialog();
            databaseConnectionWindow.ShowDialog();
        }

        private async Task InitializeDatabaseAsync(string connectionString)
        {
            try
            {
                await DBInitializer.InitializeAsync(connectionString);
                string dbConnectionString = connectionString.Replace("master", "DB_THESIS");
                using (SqlConnection connection = new SqlConnection(dbConnectionString))
                {
                    await connection.OpenAsync();
                    if (!await AdminUserExistsAsync(connection))
                    {
                        OpenAdminCreationWindow(dbConnectionString);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Logger.Error(sqlEx, "SQL Ошибка инициализации базы данных");
                MessageBox.Show($"SQL Ошибка инициализации базы данных: {sqlEx.Message}");
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

        private async Task<bool> TestConnectionStringAsync(string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    return true;
                }
            }
            catch (SqlException sqlEx)
            {
                Logger.Error(sqlEx, "Ошибка тестирования строки подключения");
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка тестирования строки подключения");
                return false;
            }
        }

        private async Task<bool> AdminUserExistsAsync(SqlConnection connection)
        {
            string query = "SELECT COUNT(*) FROM Users WHERE IsAdmin = 1";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                int adminCount = (int)await command.ExecuteScalarAsync();
                return adminCount > 0;
            }
        }

        private void OpenAdminCreationWindow(string connectionString)
        {
            var adminWindow = new AdminWindow(connectionString);
            if (adminWindow.ShowDialog() != true)
            {
                Shutdown();
            }
        }

        private void ShowWelcomeWindow()
        {
            var welcomeWindow = new WelcomeWindow();
            welcomeWindow.ShowDialog();
        }
    }
}
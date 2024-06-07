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
using ThesisProjectARM.Services.Services;
using ThesisProjectARM.Data;
using UI.Properties;
using ThesisProjectARM.Data.Repositories;

namespace UI
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly SimpleInjector.Container _container;

        public App()
        {
            _container = new SimpleInjector.Container();
            ConfigureContainer();
        }

        private void ConfigureContainer()
        {
            _container.Register<IUserRepository, UserRepository>(Lifestyle.Singleton);
            _container.Register<IDatabaseService, DataEntryService>(Lifestyle.Singleton);
            _container.Register<IWindowService, WindowService>(Lifestyle.Singleton);
            _container.Register<ManagerVM>(Lifestyle.Singleton);
            _container.Register<RegistrationVM>(Lifestyle.Singleton);
            _container.Register<SelectTableVM>(Lifestyle.Singleton);
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var dataEntryService = _container.GetInstance<IDataEntryRepository>();

            string connectionString = UI.Properties.Settings.Default.ConnectionString;
            if (string.IsNullOrEmpty(connectionString) || !await dataEntryService.TestConnectionAsync(connectionString))
            {
                OpenDatabaseConnectionWindow();
                connectionString = UI.Properties.Settings.Default.ConnectionString;
            }

            await InitializeDatabaseAsync(dataEntryService, connectionString);
            ShowWelcomeWindow();
        }

        private void OpenDatabaseConnectionWindow()
        {
            var databaseConnectionWindow = new SelectTableDialog();
            databaseConnectionWindow.ShowDialog();
        }

        private async Task InitializeDatabaseAsync(IDataEntryRepository dataEntryService, string connectionString)
        {
            try
            {
                await dataEntryService.InitializeDatabaseAsync(connectionString);
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
                UI.Properties.Settings.Default.ConnectionString = string.Empty;
                UI.Properties.Settings.Default.Save();
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
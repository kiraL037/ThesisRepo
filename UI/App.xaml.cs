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
using System.Configuration;
using ThesisProjectARM.Data.Repositories;
using System.Xml.Linq;

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
            _container.Register<IDatabaseService, DBService>(Lifestyle.Singleton);
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
            _container.Register<MainUIWindow>(Lifestyle.Transient);
            _container.Register<MainWindow>(Lifestyle.Transient);
            _container.Register<TipsWindow>(Lifestyle.Transient);
            _container.Register<SelectTableDialog>(Lifestyle.Transient);
            _container.Register<RegistrationWindow>(Lifestyle.Transient);
            _container.Register<FirstSetupWindow>(Lifestyle.Transient);
            _container.Register<ManagerWindow>(Lifestyle.Transient);
            _container.Register<DataEntry>(Lifestyle.Transient);
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
                Settings.Default.ConnectionString = string.Empty;
                Settings.Default.Save();
                Shutdown();
            }
        }

        public void SaveConnectionString(string connectionString)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (config.ConnectionStrings.ConnectionStrings["MyConnectionString"] != null)
                {
                    config.ConnectionStrings.ConnectionStrings["MyConnectionString"].ConnectionString = connectionString;
                }
                else
                {
                    config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("MyConnectionString", connectionString, "System.Data.SqlClient"));
                }
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("connectionStrings");

                Settings.Default.ConnectionString = connectionString;
                Settings.Default.Save();

                MessageBox.Show("Connection string saved successfully.");
            }
            catch (ConfigurationErrorsException ex)
            {
                MessageBox.Show("Error saving connection string: " + ex.Message);
            }
        }

        private void ShowWelcomeWindow()
        {
            var welcomeWindow = new WelcomeWindow();
            welcomeWindow.ShowDialog();
        }
    }
}
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
using UI;
using ThesisProjectARM.Core.Models;

namespace UI
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static SimpleInjector.Container _container;

        public App()
        {
            _container = new SimpleInjector.Container();
            ConfigureContainer();
        }

        private void ConfigureContainer()
        {
            _container.Register<IUserRepository, UserRepository>(Lifestyle.Singleton);
            _container.Register<IUserService, UserService>(Lifestyle.Singleton);
            _container.Register<IDatabaseService, DBService>(Lifestyle.Singleton);
            _container.Register<IWindowService, WindowService>(Lifestyle.Singleton);
            _container.Register<ManagerVM>(Lifestyle.Singleton);
            _container.Register<RegistrationVM>(Lifestyle.Singleton);
            _container.Register<AuthenticationWindowVM>(Lifestyle.Singleton);
            _container.Register<SelectTableVM>(Lifestyle.Singleton);
            _container.Register<FirstSetupWindowVM>(Lifestyle.Singleton);
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var dbService = _container.GetInstance<IDatabaseService>();
            string connectionString = Settings.Default.ConnectionString;

            if (string.IsNullOrEmpty(connectionString) || !await dbService.TestConnectionAsync(connectionString))
            {
                OpenFirstSetupWindow();
            }
            else
            {
                await InitializeDatabaseAsync(dbService, connectionString);
                ShowWelcomeWindow();
            }
            RegisterWindows();
        }

        private void RegisterWindows()
        {
            _container.Register<MainUIWindow>(Lifestyle.Transient);
            _container.Register<MainWindow>(Lifestyle.Transient);
            _container.Register<TipsWindow>(Lifestyle.Transient);
            _container.Register<SelectTableDialog>(Lifestyle.Transient);
            _container.Register<RegistrationWindow>(Lifestyle.Transient);
            _container.Register<FirstSetupWindow>(Lifestyle.Transient);
            _container.Register<ManagerWindow>(Lifestyle.Transient);
            _container.Register<DataEntry>(Lifestyle.Transient);
        }

        private void OpenFirstSetupWindow()
        {
            var firstSetupWindow = _container.GetInstance<FirstSetupWindow>();
            firstSetupWindow.ShowDialog();
        }

        private async Task InitializeDatabaseAsync(IDatabaseService dbService, string connectionString)
        {
            try
            {
                await dbService.SetupDatabaseAsync(new ConnectionModel
                {
                    ConnectionString = connectionString
                });
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

        private void ShowWelcomeWindow()
        {
            var welcomeWindow = _container.GetInstance<WelcomeWindow>();
            welcomeWindow.ShowDialog();
        }
    }
}
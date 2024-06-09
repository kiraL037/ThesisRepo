using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;
using SimpleInjector;
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

namespace UI
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static Container _container;

        public App()
        {
            _container = new Container();
            ConfigureContainer();
        }

        private void ConfigureContainer()
        {
            // Регистрация ISecurityMethods
            _container.Register<ISecurityMethods, SecurityMethods>(Lifestyle.Singleton);

            // Другие регистрации
            _container.Register<IUserRepository>(() =>
            {
                string connectionString = Settings.Default.ConnectionString;
                return new UserRepository(connectionString);
            }, Lifestyle.Singleton);

            // Регистрация фабрик
            _container.Register<IDataVisualizationVMFactory, DataVisualizationVMFactory>(Lifestyle.Singleton);
            _container.Register<IDBCRUDVMFactory, DBCRUDVMFactory>(Lifestyle.Singleton);
            _container.Register<IDataPageFactory, DataPageFactory>(Lifestyle.Singleton);
            _container.Register<IAnalysisVMFactory, AnalysisVMFactory>(Lifestyle.Singleton);

            // Регистрация других зависимостей
            _container.Register<IDBCHService, DBCHService>(Lifestyle.Singleton);
            _container.Register<IGetData, CSVDatainfo>(Lifestyle.Singleton);

            // Регистрация ViewModels
            _container.Register<ManagerVM>(Lifestyle.Singleton);
            _container.Register<AdminWindowVM>(Lifestyle.Singleton);
            _container.Register<DataPageVM>(Lifestyle.Singleton);
            _container.Register<DBCRUDVM>(Lifestyle.Singleton);
            _container.Register<TipsWindowVM>(Lifestyle.Singleton);
            _container.Register<WelcomeWindowVM>(Lifestyle.Singleton);
            _container.Register<RegistrationVM>(Lifestyle.Singleton);
            _container.Register<AuthenticationWindowVM>(Lifestyle.Singleton);
            _container.Register<SelectTableVM>(Lifestyle.Singleton);
            _container.Register<FirstSetupWindowVM>(Lifestyle.Singleton);

            // Регистрация IDataVisualizer
            _container.Register<IDataVisualizer, DataVisualizer>(Lifestyle.Singleton);

            // Регистрация MainUIVM
            _container.Register<MainUIVM>(Lifestyle.Singleton);


            // Регистрация окон
            RegisterWindows();
        }

        private void RegisterWindows()
        {
            _container.Register<WelcomeWindow>(Lifestyle.Transient);
            _container.Register<AdminWindow>(Lifestyle.Transient);
            _container.Register<MainUIWindow>(Lifestyle.Transient);
            _container.Register<TipsWindow>(Lifestyle.Transient);
            _container.Register<SelectTableDialog>(Lifestyle.Transient);
            _container.Register<RegistrationWindow>(Lifestyle.Transient);
            _container.Register<FirstSetupWindow>(Lifestyle.Transient);
            _container.Register<ManagerWindow>(Lifestyle.Transient);
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
                ShowManagerOrWelcomeWindow();
            }
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
            var userRepository = _container.GetInstance<IUserRepository>();
            bool adminExists = await userRepository.AdminUserExistsAsync();

            if (adminExists)
            {
                var adminWindow = _container.GetInstance<AdminWindow>();
                adminWindow.ShowDialog();
            }
            else
            {
                var welcomeWindow = _container.GetInstance<WelcomeWindow>();
                welcomeWindow.ShowDialog();
            }
        }

        // Фабричный метод для AnalysisVM
        public interface IAnalysisVMFactory
        {
            AnalysisVM Create(DataTable dataTable);
        }

        public class AnalysisVMFactory : IAnalysisVMFactory
        {
            public AnalysisVM Create(DataTable dataTable)
            {
                return new AnalysisVM(dataTable);
            }
        }

        // Интерфейс и реализация фабрики для DBDataInfo
        public interface IDBDataInfoFactory
        {
            DBDataInfo Create(string connectionString);
        }

        public class DBDataInfoFactory : IDBDataInfoFactory
        {
            public DBDataInfo Create(string connectionString)
            {
                return new DBDataInfo(connectionString);
            }
        }

        // Интерфейс и реализация фабрики для CSVDatainfo
        public interface ICSVDataInfoFactory
        {
            CSVDatainfo Create(string filePath);
        }

        public class CSVDataInfoFactory : ICSVDataInfoFactory
        {
            public CSVDatainfo Create(string filePath)
            {
                return new CSVDatainfo(filePath);
            }
        }

        // Интерфейс и реализация фабрики для DataVisualizationVM
        public interface IDataVisualizationVMFactory
        {
            DataVisualizationVM Create(DataTable dataTable);
        }

        public class DataVisualizationVMFactory : IDataVisualizationVMFactory
        {
            private readonly IDataVisualizer _dataVisualizer;

            public DataVisualizationVMFactory(IDataVisualizer dataVisualizer)
            {
                _dataVisualizer = dataVisualizer;
            }

            public DataVisualizationVM Create(DataTable dataTable)
            {
                return new DataVisualizationVM(_dataVisualizer, dataTable);
            }
        }

        public interface IDataPageFactory
        {
            DataPage Create();
        }

        public class DataPageFactory : IDataPageFactory
        {
            public DataPage Create()
            {
                return new DataPage();
            }
        }

        public interface IDBCRUDVMFactory
        {
            DBCRUDVM Create(IDataInfo dataInfo);
        }

        public class DBCRUDVMFactory : IDBCRUDVMFactory
        {
            public DBCRUDVM Create(IDataInfo dbDataInfo)
            {
                return new DBCRUDVM(dbDataInfo);
            }
        }
    }
}
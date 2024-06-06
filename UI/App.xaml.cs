using Core.Interfaces;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ThesisProjectARM.Core.Interfaces;
using ThesisProjectARM.Services.Services;
using ThesisProjectARM.UI.ViewModels;

namespace UI
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<IDataService, DataService>();
            services.AddSingleton<IDataAnalyzer, DataAnalyzer>();
            services.AddSingleton<IDataVisualizer, DataVisualizer>();
            services.AddSingleton<IDataExporter, DataExporter>();
            services.AddTransient<MainUIWindowVM>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = new MainWindow();
            var mainWindowVM = serviceProvider.GetRequiredService<MainUIWindowVM>();
            mainWindow.DataContext = mainWindowVM;
            mainWindow.Show();
        }
    }
}

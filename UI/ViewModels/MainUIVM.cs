using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ThesisProjectARM.Core.Interfaces;
using ThesisProjectARM.UI.Views.Pages;
using UI;

namespace ThesisProjectARM.UI.ViewModels
{
    public class MainUIWindowVM
    {
        private readonly IDataService dataService;
        private readonly IDataAnalyzer dataAnalyzer;
        private readonly IDataVisualizer dataVisualizer;
        private readonly IDataExporter dataExporter;

        public MainUIWindowVM(IDataService dataService, IDataAnalyzer dataAnalyzer, IDataVisualizer dataVisualizer, IDataExporter dataExporter)
        {
            this.dataService = dataService;
            this.dataAnalyzer = dataAnalyzer;
            this.dataVisualizer = dataVisualizer;
            this.dataExporter = dataExporter;

            LoadDataCommand = new RelayCommand(LoadData);
            AnalyzeDataCommand = new RelayCommand(AnalyzeData);
            VisualizeDataCommand = new RelayCommand(VisualizeData);
            ExportDataCommand = new RelayCommand(ExportData);
            SaveChangesCommand = new RelayCommand(SaveChanges);
        }

        public DataTable LoadedData { get; private set; }

        public ICommand LoadDataCommand { get; }
        public ICommand AnalyzeDataCommand { get; }
        public ICommand VisualizeDataCommand { get; }
        public ICommand ExportDataCommand { get; }
        public ICommand SaveChangesCommand { get; }

        private void LoadData()
        {
            var dataPageVM = new DataPageVM();
            var dataPage = new DataPage
            {
                DataContext = dataPageVM
            };

            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Content = dataPage;

            dataPageVM.LoadDataCommand.Execute(null);
        }

        private void AnalyzeData(object parameter)
        {
            try
            {
                string analysisType = parameter as string;
                var result = dataAnalyzer.AnalyzeData(LoadedData, analysisType);
                // Обработка результата анализа
            }
            catch (Exception ex)
            {
                // Логирование ошибки и вывод сообщения пользователю
                MessageBox.Show($"Ошибка при анализе данных: {ex.Message}");
            }
        }

        private void VisualizeData(object parameter)
        {
            try
            {
                string columnName = parameter as string;
                dataVisualizer.PlotData(LoadedData, columnName);
            }
            catch (Exception ex)
            {
                // Логирование ошибки и вывод сообщения пользователю
                MessageBox.Show($"Ошибка при визуализации данных: {ex.Message}");
            }
        }

        private void ExportData(object parameter)
        {
            try
            {
                string filePath = parameter as string;
                dataExporter.ExportData(LoadedData, filePath);
            }
            catch (Exception ex)
            {
                // Логирование ошибки и вывод сообщения пользователю
                MessageBox.Show($"Ошибка при экспорте данных: {ex.Message}");
            }
        }

        private void SaveChanges(object parameter)
        {
            string filePath = parameter as string;
            dataService.SaveData(LoadedData, filePath);
        }
    }
}
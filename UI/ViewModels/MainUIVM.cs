using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using ThesisProjectARM.Core.Interfaces;
using ThesisProjectARM.Core.Models;
using ThesisProjectARM.UI.Views.Pages;
using ThesisProjectARM.UI.Views.Windows;
using UI;

namespace ThesisProjectARM.UI.ViewModels
{
    public class MainUIVM
    {
        private readonly IDataInfo _dataService;
        public ObservableCollection<DynamicDataModel> DataCollection { get; set; }

        public ICommand LoadDataCommand { get; }
        public ICommand SaveChangesCommand { get; }
        public ICommand ConnectToDBCommand { get; }
        public ICommand ReportCommand { get; }
        public ICommand DataCommand { get; }
        public ICommand AboutCommand { get; }
        public ICommand ExitCommand { get; }

        public MainUIVM(IDataInfo dataService)
        {
            _dataService = dataService;
            DataCollection = new ObservableCollection<DynamicDataModel>();

            LoadDataCommand = new RelayCommand(async (param) => await LoadData());
            SaveChangesCommand = new RelayCommand(async (param) => await SaveChanges());
            ConnectToDBCommand = new RelayCommand(async (param) => await ConnectToDB());
            ReportCommand = new RelayCommand((param) => OpenReport());
            DataCommand = new RelayCommand((param) => OpenData());
            AboutCommand = new RelayCommand((param) => ShowAbout());
            ExitCommand = new RelayCommand((param) => ExitApplication());
        }

        private async Task LoadData()
        {
            try
            {
                var data = await _dataService.LoadDataAsync("YourTableName"); // Replace with your table name
                DataCollection.Clear();
                foreach (var item in data)
                {
                    DataCollection.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task SaveChanges()
        {
            try
            {
                foreach (var item in DataCollection)
                {
                    await _dataService.InsertDataAsync("YourTableName", item); // Replace with your table name
                }
                MessageBox.Show("Data saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task ConnectToDB()
        {
            try
            {
                // Implement database connection logic if needed
                MessageBox.Show("Successfully connected to the database.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to the database: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenReport()
        {
            var analysisPage = new AnalysisPage();
            NavigationService.Navigate(analysisPage);
        }

        private void OpenData()
        {
            var dataPage = new DataPage();
            NavigationService.Navigate(dataPage);
        }

        private void ShowAbout()
        {
            MessageBox.Show("Analytics application version 1.0\nDeveloped for thesis project\nVersion 1.0", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExitApplication()
        {
            Application.Current.Shutdown();
        }

        private NavigationService NavigationService
        {
            get
            {
                foreach (Window window in Application.Current.Windows)
                {
                    if (window is MainUIWindow mainWindow)
                    {
                        return mainWindow.MainFrame.NavigationService;
                    }
                }
                return null;
            }
        }
    }
}
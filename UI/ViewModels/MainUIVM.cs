using Core.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Core.Models;
using Services.Services;
using UI.Views.Pages;
using UI.Views.Windows;
using UI.ViewModels;

namespace UI.ViewModels
{
    public class MainUIVM : ViewModelBase 
    {
        private IGetData _csvDataInfo;
        private IDBCHService _dbCHService;
        private DataTable _dataTable;
        private Frame _mainFrame;
        private DBCRUDVM _dbCrudVm;
        private string _selectedTableName;
        private string _connectionString;

        public DataTable DataTable
        {
            get => _dataTable;
            set
            {
                _dataTable = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<DynamicDataModel> DataCollection { get; set; }

        public ICommand LoadDataCommand { get; }
        public ICommand SaveChangesCommand { get; }
        public ICommand ConnectToDBCommand { get; }
        public ICommand AnalysisCommand { get; }
        public ICommand DataCommand { get; }
        public ICommand AboutCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand OpenFileCommand { get; }
        public ICommand AddDataCommand { get; }
        public ICommand EditDataCommand { get; }
        public ICommand DeleteDataCommand { get; }

        public MainUIVM()
        {
            DataCollection = new ObservableCollection<DynamicDataModel>();
            _dbCHService = new DBCHService();

            LoadDataCommand = new RelayCommand(async (param) => await LoadDataFromCsv());
            SaveChangesCommand = new RelayCommand(async (param) => await SaveChanges());
            ConnectToDBCommand = new RelayCommand(async (param) => await ConnectToDB());
            AnalysisCommand = new RelayCommand((param) => NavigateToAnalysisPage());
            DataCommand = new RelayCommand((param) => NavigateToDataPage());
            AboutCommand = new RelayCommand((param) => ShowAbout());
            ExitCommand = new RelayCommand((param) => ExitApplication());
            OpenFileCommand = new RelayCommand((param) => OpenFile());
            AddDataCommand = new RelayCommand(async (param) => await AddData());
            EditDataCommand = new RelayCommand(async (param) => await EditData());
            DeleteDataCommand = new RelayCommand(async (param) => await DeleteData());

            NavigateToDataPage();
        }

        private void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                _csvDataInfo = new CSVDatainfo(filePath);
            }
        }

        private async Task LoadDataFromCsv()
        {
            if (_csvDataInfo == null)
            {
                MessageBox.Show("Please select a file first.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                DataTable = await _csvDataInfo.GetDataAsync();
                var dataPage = new DataPage();
                dataPage.LoadData(DataTable);
                NavigateToPage(dataPage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data from CSV: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task SaveChanges()
        {
            try
            {
                if (_csvDataInfo != null)
                {
                    // Save changes to CSV file
                    await _csvDataInfo.SaveDataAsync(DataTable);
                }
                else if (_dbCrudVm != null)
                {
                    foreach (var item in DataCollection)
                    {
                        await _dbCrudVm.InsertData(_selectedTableName, item);
                    }
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
            var dialog = new SelectTableDialog();
            if (dialog.ShowDialog() == true)
            {
                var vm = dialog.DataContext as SelectTableVM;
                var connectionModel = new ConnectionModel
                {
                    Server = vm.Server,
                    Database = vm.Database,
                    Username = vm.User,
                    Password = vm.Password
                };

                _connectionString = _dbCHService.BuildConnectionString(connectionModel);
                bool isConnected = await _dbCHService.TestConnectionAsync(_connectionString);

                if (isConnected)
                {
                    MessageBox.Show("Successfully connected to the database.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    _selectedTableName = vm.SelectedTable;
                    _dbCrudVm = new DBCRUDVM(new DBDataInfo(_connectionString));
                    DataTable = await _dbCHService.LoadDataAsync(_connectionString, _selectedTableName);
                    var dataPage = new DataPage();
                    dataPage.LoadData(DataTable);
                    NavigateToPage(dataPage);
                }
                else
                {
                    MessageBox.Show("Failed to connect to the database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void NavigateToAnalysisPage()
        {
            var analysisPage = new AnalysisPage();
            var analysisVM = new AnalysisVM(DataTable);
            analysisPage.DataContext = analysisVM;
            NavigateToPage(analysisPage);
        }

        private void NavigateToDataPage()
        {
            var dataPage = new DataPage();
            NavigateToPage(dataPage);
        }

        private void ShowAbout()
        {
            MessageBox.Show("Analytics application version 1.0\nDeveloped for thesis project\nVersion 1.0", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExitApplication()
        {
            Application.Current.Shutdown();
        }

        private async Task AddData()
        {
            var newData = new DynamicDataModel();
            if (_csvDataInfo != null)
            {
                DataTable.Rows.Add(newData.ToDataRow(DataTable));
                await _csvDataInfo.SaveDataAsync(DataTable);
            }
            else if (_dbCrudVm != null)
            {
                await _dbCrudVm.InsertData(_selectedTableName, newData);
            }
        }

        private async Task EditData()
        {
            var selectedData = DataCollection.FirstOrDefault();
            if (selectedData != null)
            {
                if (_csvDataInfo != null)
                {
                    var row = DataTable.Rows.Cast<DataRow>().FirstOrDefault(r => r.Equals(selectedData.ToDataRow(DataTable)));
                    if (row != null)
                    {
                        row.ItemArray = selectedData.ToDataRow(DataTable).ItemArray;
                        await _csvDataInfo.SaveDataAsync(DataTable);
                    }
                }
                else if (_dbCrudVm != null)
                {
                    await _dbCrudVm.UpdateData(_selectedTableName, selectedData, selectedData.Id);
                }
            }
        }

        private async Task DeleteData()
        {
            var selectedData = DataCollection.FirstOrDefault();
            if (selectedData != null)
            {
                if (_csvDataInfo != null)
                {
                    var row = DataTable.Rows.Cast<DataRow>().FirstOrDefault(r => r.Equals(selectedData.ToDataRow(DataTable)));
                    if (row != null)
                    {
                        DataTable.Rows.Remove(row);
                        await _csvDataInfo.SaveDataAsync(DataTable);
                    }
                }
                else if (_dbCrudVm != null)
                {
                    await _dbCrudVm.DeleteData(_selectedTableName, selectedData.Id);
                }
            }
        }

        private void NavigateToVisualizationPage()
        {
            var visualizationPage = new DataVisualizationPage();
            var dataVisualizer = new DataVisualizer(); 
            var visualizationVM = new DataVisualizationVM(dataVisualizer, DataTable);
            visualizationPage.DataContext = visualizationVM;
            NavigateToPage(visualizationPage);
        }

        private void NavigateToPage(Page page)
        {
            var navigationService = NavigationService;
            if (navigationService != null)
            {
                navigationService.Navigate(page);
            }
            else
            {
                MessageBox.Show("Navigation service is not available.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        protected new void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public new event PropertyChangedEventHandler PropertyChanged;
    }
}
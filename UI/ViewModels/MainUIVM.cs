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
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;

namespace UI.ViewModels
{
    public class MainUIVM : ViewModelBase
    {
        private IGetData _csvDataInfo;
        private IDBCHService _dbCHService;
        private DataTable _dataTable;
        private string _selectedTableName;
        private string _connectionString;

        public DataTable DataTable
        {
            get => _dataTable;
            set
            {
                _dataTable = value;
                OnPropertyChanged();
                DataCollection.Clear();
                foreach (DataRow row in _dataTable.Rows)
                {
                    DataCollection.Add(DynamicDataModel.FromDataRow(row));
                }
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
        public ICommand AddFileCommand { get; }
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
            AddFileCommand = new RelayCommand(async (param) => await AddFile());
            EditDataCommand = new RelayCommand((param) => EditData());
            DeleteDataCommand = new RelayCommand((param) => DeleteData());

            NavigateToDataPage();
        }

        private async Task LoadDataFromCsv()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "CSV файлы (*.csv)|*.csv|Все файлы (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",
                    HasHeaderRecord = true
                };

                try
                {
                    using (var reader = new StreamReader(filePath))
                    using (var csv = new CsvReader(reader, config))
                    {
                        DataTable dataTable = new DataTable();

                        // Считываем заголовки
                        csv.Read();
                        csv.ReadHeader();

                        if (csv.HeaderRecord != null)
                        {
                            foreach (var header in csv.HeaderRecord)
                            {
                                dataTable.Columns.Add(header);
                            }
                        }

                        // Читаем строки данных и добавляем их в DataTable
                        var records = csv.GetRecords<dynamic>();

                        await Task.Run(() =>
                        {
                            foreach (var record in records)
                            {
                                if (record == null) continue;

                                var dictionary = (IDictionary<string, object>)record;

                                var row = dataTable.NewRow();
                                foreach (var header in csv.HeaderRecord)
                                {
                                    if (dictionary.TryGetValue(header, out var value))
                                    {
                                        row[header] = value ?? DBNull.Value;
                                    }
                                }
                                dataTable.Rows.Add(row);
                            }
                        });

                        DataTable = dataTable;
                        var dataPage = new DataPage();
                        dataPage.LoadData(DataTable);
                        NavigateToPage(dataPage);
                    }

                    MessageBox.Show("Данные успешно загружены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки данных из файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task SaveChanges()
        {
            try
            {
                if (_csvDataInfo != null)
                {
                    await _csvDataInfo.SaveDataAsync(DataTable);
                }
                else if (_dbCHService != null)
                {
                    foreach (var item in DataCollection)
                    {
                        await _dbCHService.InsertDataAsync(_selectedTableName, item);
                    }
                }
                MessageBox.Show("Данные успешно сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task ConnectToDB()
        {
            var selectTableVM = new SelectTableVM();
            var selectTableDialog = new SelectTableDialog(selectTableVM);
            if (selectTableDialog.ShowDialog() == true)
            {
                var vm = selectTableDialog.DataContext as SelectTableVM;
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
                    MessageBox.Show("Успешное подключение к БД", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    _selectedTableName = vm.SelectedTable;
                    DataTable = await _dbCHService.LoadDataAsync(_connectionString, _selectedTableName);
                    var dataPage = new DataPage();
                    dataPage.LoadData(DataTable);
                    NavigateToPage(dataPage);
                }
                else
                {
                    MessageBox.Show("Ошибка подключения к БД", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
            MessageBox.Show("Аналитическое приложение версии 1.0\nРазработано для дипломной работы\nВерсия 1.0", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ExitApplication()
        {
            Application.Current.Shutdown();
        }

        private async Task AddFile()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "CSV файлы (*.csv)|*.csv|Все файлы (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ",",
                    HasHeaderRecord = true
                };

                try
                {
                    using (var reader = new StreamReader(filePath))
                    using (var csv = new CsvReader(reader, config))
                    {
                        DataTable dataTable = new DataTable();

                        // Считываем заголовки
                        csv.Read();
                        csv.ReadHeader();
                        if (csv.HeaderRecord != null)
                        {
                            foreach (var header in csv.HeaderRecord)
                            {
                                dataTable.Columns.Add(header);
                            }
                        }

                        // Читаем строки данных и добавляем их в DataTable
                        var records = csv.GetRecords<dynamic>();

                        await Task.Run(() =>
                        {
                            foreach (var record in records)
                            {
                                if (record == null) continue;

                                var dictionary = (IDictionary<string, object>)record;

                                var row = dataTable.NewRow();
                                foreach (var header in csv.HeaderRecord)
                                {
                                    if (dictionary.TryGetValue(header, out var value))
                                    {
                                        row[header] = value ?? DBNull.Value;
                                    }
                                }
                                dataTable.Rows.Add(row);
                            }
                        });

                        DataTable = dataTable;
                        var dataPage = new DataPage();
                        dataPage.LoadData(DataTable);
                        NavigateToPage(dataPage);
                    }

                    MessageBox.Show("Данные успешно загружены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки данных из файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EditData()
        {
        }

        private void DeleteData()
        {
            
        }

        private void NavigateToPage(Page page)
        {
            var navigationService = ((MainUIWindow)Application.Current.MainWindow).MainFrame.NavigationService;
            navigationService.Navigate(page);
        }
    }
}
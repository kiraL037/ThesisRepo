using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ThesisProjectARM.Services.Services;

namespace ThesisProjectARM.UI.ViewModels
{
    public class DataPageVM : ViewModelBase
    {
        private ObservableCollection<DataRow> _dataRows;
        private DataTable _dataTable;

        public ObservableCollection<DataRow> DataRows
        {
            get { return _dataRows; }
            set
            {
                _dataRows = value;
                OnPropertyChanged(nameof(DataRows));
            }
        }

        public ICommand LoadDataCommand { get; }

        public DataPageVM()
        {
            LoadDataCommand = new RelayCommand(async () => await LoadDataAsync());
        }

        private async Task LoadDataAsync()
        {
            // Assuming you have a method to get the file path from the main interface
            string filePath = "path_to_your_csv_file"; // Replace with actual logic to get the file path

            if (File.Exists(filePath))
            {
                var csvDataInfo = new CSVDatainfo(filePath);
                _dataTable = await csvDataInfo.GetDataAsync();
                DataRows = new ObservableCollection<DataRow>(_dataTable.AsEnumerable());
            }
        }
    }
}
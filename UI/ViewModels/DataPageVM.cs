using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.IO;
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
            LoadDataCommand = new RelayCommand(async param => await LoadDataAsync(param), param => CanLoadData(param));
        }

        private bool CanLoadData(object param)
        {
            return param is string filePath && File.Exists(filePath);
        }

        public async Task LoadDataAsync(object param)
        {
            if (param is string filePath && File.Exists(filePath))
            {
                var csvDataInfo = new CSVDatainfo(filePath);
                _dataTable = await csvDataInfo.GetDataAsync();
                DataRows = new ObservableCollection<DataRow>(_dataTable.AsEnumerable());
            }
        }
    }
}
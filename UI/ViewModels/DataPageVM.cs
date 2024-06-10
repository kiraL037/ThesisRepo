using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Services.Services;
using System.Windows.Navigation;
using UI.Views.Pages;
using static UI.App;

namespace UI.ViewModels
{
    public class DataPageVM : ViewModelBase
    {
        private ObservableCollection<DataRow> _dataRows;
        private DataTable _dataTable;
        private MainUIVM _mainUIVM;

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
        public ICommand GoToAnalysisPageCommand { get; }

        public DataPageVM(MainUIVM mainUIVM)
        {
            _mainUIVM = mainUIVM;
            LoadDataCommand = new RelayCommand(async param => await LoadDataAsync(param), param => CanLoadData(param));
            GoToAnalysisPageCommand = new RelayCommand(param => NavigateToAnalysisPage());
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
                _mainUIVM.DataTable = _dataTable;
            }
        }

        public void NavigateToAnalysisPage()
        {
            _mainUIVM.NavigateToAnalysisPage();
        }
    }
}
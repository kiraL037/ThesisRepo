using Core.Interfaces;
using OxyPlot;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UI.ViewModels
{
    public class DataVisualizationVM : ViewModelBase
    {
        private readonly IDataVisualizer _dataVisualizerService;
        private DataTable _dataTable;
        private string _selectedColumnName;
        private PlotModel _plotModel;

        public ObservableCollection<string> ColumnNames { get; set; }
        public string SelectedColumnName
        {
            get => _selectedColumnName;
            set
            {
                _selectedColumnName = value;
                OnPropertyChanged();
            }
        }

        public PlotModel PlotModel
        {
            get => _plotModel;
            set
            {
                _plotModel = value;
                OnPropertyChanged();
            }
        }

        public ICommand VisualizeCommand { get; }

        public DataVisualizationVM(IDataVisualizer dataVisualizerService, DataTable dataTable)
        {
            _dataVisualizerService = dataVisualizerService;
            _dataTable = dataTable;
            ColumnNames = new ObservableCollection<string>(dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
            VisualizeCommand = new RelayCommand(async (param) => await VisualizeData());
        }

        public async Task VisualizeData()
        {
            if (!string.IsNullOrEmpty(_selectedColumnName))
            {
                PlotModel = _dataVisualizerService.PlotData(_dataTable, _selectedColumnName);
            }
        }
    }
}
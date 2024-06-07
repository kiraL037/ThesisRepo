using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisProjectARM.Core.Models;
using ThesisProjectARM.Core.Interfaces;

namespace UI.ViewModels
{
    public class DBCRUDVM : INotifyPropertyChanged
    {
        private readonly IDataInfo _dataInfo;
        public ObservableCollection<DynamicDataModel> Data { get; set; }

        public DBCRUDVM(IDataInfo dataInfo)
        {
            _dataInfo = dataInfo;
            Data = new ObservableCollection<DynamicDataModel>();
        }

        public async Task LoadData(string tableName)
        {
            Data.Clear();
            var data = await _dataInfo.LoadDataAsync(tableName);
            foreach (var item in data)
            {
                Data.Add(item);
            }
        }

        public async Task InsertData(string tableName, DynamicDataModel model)
        {
            await _dataInfo.InsertDataAsync(tableName, model);
            await LoadData(tableName);
        }

        public async Task UpdateData(string tableName, DynamicDataModel model, int id)
        {
            await _dataInfo.UpdateDataAsync(tableName, model, id);
            await LoadData(tableName);
        }

        public async Task DeleteData(string tableName, int id)
        {
            await _dataInfo.DeleteDataAsync(tableName, id);
            await LoadData(tableName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

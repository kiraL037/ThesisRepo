﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ThesisProjectARM.UI.ViewModels
{
    public class SelectTableVM : ViewModelBase
    {
        private ObservableCollection<string> _tables;
        public ObservableCollection<string> Tables
        {
            get { return _tables; }
            set
            {
                _tables = value;
                OnPropertyChanged(nameof(Tables));
            }
        }

        private string _selectedTable;
        public string SelectedTable
        {
            get { return _selectedTable; }
            set
            {
                _selectedTable = value;
                OnPropertyChanged(nameof(SelectedTable));
            }
        }

        private ICommand _selectTableCommand;
        public ICommand SelectTableCommand
        {
            get
            {
                if (_selectTableCommand == null)
                {
                    _selectTableCommand = new RelayCommand(param => SelectTable(), param => CanSelectTable());
                }
                return _selectTableCommand;
            }
        }

        private bool CanSelectTable()
        {
            // Logic to enable or disable the command
            return !string.IsNullOrEmpty(SelectedTable);
        }

        private void SelectTable()
        {
            // Logic to select table
        }

        public SelectTableVM()
        {
            Tables = new ObservableCollection<string>();
            // Logic to populate Tables collection
        }
    }
}
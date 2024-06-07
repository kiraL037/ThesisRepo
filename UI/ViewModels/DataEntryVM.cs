using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ThesisProjectARM.UI.ViewModels;

namespace UI.ViewModels
{
    public class DataEntryVM : ViewModelBase
    {
        private string _dataEntryField;
        public string DataEntryField
        {
            get { return _dataEntryField; }
            set
            {
                _dataEntryField = value;
                OnPropertyChanged(nameof(DataEntryField));
            }
        }

        private ICommand _submitDataCommand;
        public ICommand SubmitDataCommand
        {
            get
            {
                if (_submitDataCommand == null)
                {
                    _submitDataCommand = new RelayCommand(param => SubmitData(), param => CanSubmitData());
                }
                return _submitDataCommand;
            }
        }

        private bool CanSubmitData()
        {
            // Logic to enable or disable the command
            return !string.IsNullOrEmpty(DataEntryField);
        }

        private void SubmitData()
        {
            // Logic to submit data
        }
    }
}
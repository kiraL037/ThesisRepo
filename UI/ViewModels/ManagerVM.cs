using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ThesisProjectARM.UI.ViewModels
{
    public class ManagerVM : ViewModelBase
    {
        private ObservableCollection<ManagerItem> _managerItems;
        public ObservableCollection<ManagerItem> ManagerItems
        {
            get { return _managerItems; }
            set
            {
                _managerItems = value;
                OnPropertyChanged(nameof(ManagerItems));
            }
        }

        private ICommand _loadItemsCommand;
        public ICommand LoadItemsCommand
        {
            get
            {
                if (_loadItemsCommand == null)
                {
                    _loadItemsCommand = new RelayCommand(param => LoadItems(), param => CanLoadItems());
                }
                return _loadItemsCommand;
            }
        }

        private bool CanLoadItems()
        {
            // Logic to enable or disable the command
            return true;
        }

        private void LoadItems()
        {
            // Logic to load items
        }

        public ManagerVM()
        {
            ManagerItems = new ObservableCollection<ManagerItem>();
        }
    }

    public class ManagerItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
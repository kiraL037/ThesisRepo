using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisProjectARM.UI.ViewModels
{
    public class TipsWindowVM : ViewModelBase
    {
        private ObservableCollection<string> _tips;
        public ObservableCollection<string> Tips
        {
            get { return _tips; }
            set
            {
                _tips = value;
                OnPropertyChanged(nameof(Tips));
            }
        }

        public TipsWindowVM()
        {
            Tips = new ObservableCollection<string>();
            // Logic to populate Tips collection
        }
    }
}
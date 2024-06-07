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
            set { _tips = value; OnPropertyChanged(nameof(Tips)); }
        }

        public TipsWindowVM()
        {
            Tips = new ObservableCollection<string>
        {
            "Tip 1: Follow the instructions.",
            "Tip 2: Save your work regularly.",
            "Tip 3: Ensure data is backed up."
        };
        }
    }
}
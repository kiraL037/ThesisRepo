using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisProjectARM.UI.ViewModels
{
    public class WelcomeWindowVM : ViewModelBase
    {
        private string _welcomeMessage;
        public string WelcomeMessage
        {
            get { return _welcomeMessage; }
            set
            {
                _welcomeMessage = value;
                OnPropertyChanged(nameof(WelcomeMessage));
            }
        }

        public WelcomeWindowVM()
        {
            WelcomeMessage = "Welcome to the application!";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisProjectARM.Core.Interfaces
{
    public interface IWindowService
    {
        bool IsWindowsAuth();
        void CreateManagerAccount();
        void CloseWindow(bool? dialogResult = null);
        void ShowMessage(string message);
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisProjectARM.Core.Interfaces;

namespace ThesisProjectARM.Services.Services
{
    public class WindowService : IWindowService
    {
        public void OpenWindow(Window window)
        {
            window.ShowDialog();
        }

        public void CloseWindow(Window window)
        {
            window.Close();
        }
    }
}
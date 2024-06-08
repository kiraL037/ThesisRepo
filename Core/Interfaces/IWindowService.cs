using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ThesisProjectARM.Core.Interfaces
{
    public interface IWindowService
    {
        void ShowWindow<T>() where T : Window, new();
        void CloseWindow<T>() where T : Window;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisProjectARM.Core.Interfaces;
using System.Windows;

namespace ThesisProjectARM.Services.Services
{
    public class WindowService : IWindowService
    {
        public bool IsWindowsAuth()
        {
            if (Application.Current.MainWindow is FirstSetupWindow window)
            {
                return window.WindowsAuthRButton.IsChecked == true;
            }
            return false;
        }

        public void CreateManagerAccount()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                RegistrationWindow registrationWindow = new RegistrationWindow(isManager: true);
                registrationWindow.ShowDialog();
            });
        }

        public void CloseWindow(bool? dialogResult = null)
        {
            var window = Application.Current.Windows[0];
            if (window != null)
            {
                window.DialogResult = dialogResult;
                window.Close();
            }
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
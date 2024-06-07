using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisProjectARM.Core.Interfaces;
using System.Windows;
using SimpleInjector;

namespace ThesisProjectARM.Services.Services
{
    public class WindowService : IWindowService
    {
        private readonly Func<FirstSetupWindow> _firstSetupWindowFactory;
        private readonly Func<RegistrationWindow> _registrationWindowFactory;

        public WindowService(Func<FirstSetupWindow> firstSetupWindowFactory, Func<RegistrationWindow> registrationWindowFactory)
        {
            _firstSetupWindowFactory = firstSetupWindowFactory;
            _registrationWindowFactory = registrationWindowFactory;
        }

        public bool IsWindowsAuth()
        {
            var window = _firstSetupWindowFactory();
            return window.WindowsAuthRButton.IsChecked == true;
        }

        public void CreateManagerAccount()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var registrationWindow = _registrationWindowFactory();
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
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
        private readonly Dictionary<Type, Type> _windows;

        public WindowService()
        {
            _windows = new Dictionary<Type, Type>
            {
                { typeof(AuthenticationWindow), typeof(AuthenticationWindow) },
                { typeof(RegistrationWindow), typeof(RegistrationWindow) },
                { typeof(WelcomeWindow), typeof(WelcomeWindow) },
                { typeof(MainWindow), typeof(MainWindow) }
            };
        }

        public void ShowWindow<T>() where T : Window, new()
        {
            var window = new T();
            window.Show();
        }

        public void CloseWindow<T>() where T : Window
        {
            var window = Application.Current.Windows.OfType<T>().FirstOrDefault();
            window?.Close();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ThesisProjectARM.Core.Interfaces;
using ThesisProjectARM.UI.Views.Windows;

namespace ThesisProjectARM.UI.ViewModels
{
    public class AdminWindowVM : ViewModelBase
    {
        private readonly IUserService _userService;

        public string Username { get; set; }
        public string Password { get; set; }
        public ICommand SaveCommand { get; }

        public AdminWindowVM(IUserService userService)
        {
            _userService = userService;
            SaveCommand = new RelayCommand(async (param) => await SaveAdmin());
        }

        private async Task SaveAdmin()
        {
            await _userService.RegisterUserAsync(Username, Password, true);
            // Закрытие окна после сохранения администратора
            CloseWindow();
            // Открытие окна аутентификации
            var loginWindow = new AuthenticationWindow();
            loginWindow.Show();
        }

        private void CloseWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}
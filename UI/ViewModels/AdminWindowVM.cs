using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ThesisProjectARM.Core.Interfaces;

namespace ThesisProjectARM.UI.ViewModels
{
    public class AdminWindowVM : ViewModelBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IWindowService _windowService;

        public string Username { get; set; }
        public string Password { get; set; }
        public ICommand SaveCommand { get; }

        public AdminWindowVM(IUserRepository userRepository, IWindowService windowService)
        {
            _userRepository = userRepository;
            _windowService = windowService;
            SaveCommand = new RelayCommand(async (param) => await SaveAdmin());
        }

        private async Task SaveAdmin()
        {
            // Пример асинхронного кода
            await Task.Delay(1000);
        }
    }
}
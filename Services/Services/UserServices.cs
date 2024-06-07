using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisProjectARM.Core.Interfaces;
using ThesisProjectARM.Core.Models;

namespace ThesisProjectARM.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user != null)
            {
                var hashedPassword = SecurityMethods.HashPassword(password, user.Salt);
                return SecurityMethods.VerifyPassword(password, user.PasswordHash, user.Salt);
            }
            return false;
        }

        public async Task<bool> RegisterUserAsync(string username, string password, bool isAdmin)
        {
            if (await _userRepository.UserExistsAsync(username))
            {
                throw new Exception("User already exists.");
            }

            var salt = SecurityMethods.GenerateSalt();
            var hashedPassword = SecurityMethods.HashPassword(password, salt);

            var user = new User
            {
                Username = username,
                PasswordHash = hashedPassword,
                Salt = salt,
                IsAdmin = isAdmin
            };

            await _userRepository.CreateUserAsync(user);
            return true; // Возвращаем true, если регистрация прошла успешно
        }
    }
}
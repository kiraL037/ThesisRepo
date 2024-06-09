using Core.Interfaces;
using System;
using System.Threading.Tasks;
using Core.Models;

namespace Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISecurityMethods _securityMethods;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;

        }

        public async Task<bool> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user != null)
            {
                return _securityMethods.VerifyPassword(password, user.PasswordHash);
            }
            return false;
        }

        public async Task<bool> RegisterUserAsync(string username, string password, bool isAdmin)
        {
            if (await _userRepository.UserExistsAsync(username))
            {
                throw new Exception("User already exists.");
            }

            var hashedPassword = _securityMethods.HashPassword(password, out string salt);

            var user = new User
            {
                Username = username,
                PasswordHash = hashedPassword,
                IsAdmin = isAdmin
            };

            await _userRepository.CreateUserAsync(user);
            return true;
        }
    }
}
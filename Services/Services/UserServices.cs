using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                return user.PasswordHash == hashedPassword;
            }
            return false;
        }

        public async Task RegisterUserAsync(string username, string password, bool isAdmin)
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
        }
    }
}

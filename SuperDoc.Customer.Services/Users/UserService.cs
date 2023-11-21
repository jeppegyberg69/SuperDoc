using SuperDoc.Customer.Repositories.Entities.Users;
using SuperDoc.Customer.Repositories.Users;

namespace SuperDoc.Customer.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }


        public async Task<int> AddUserAsync(string firstName, string lastName, string email, string password, Roles role)
        {
            var user = await userRepository.GetUserByEmailAsync(email);

            if (user != null)
            {
                return 1;
            }


            User newUser = new User()
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = email,
                Role = role,
                IsUserSignedUp = true,
                IsDisabled = false,
                DateCreated = DateTime.UtcNow
            };

            newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

            await userRepository.AddUserAsync(newUser);

            return 0;
        }

        public async Task<User?> GetUserByEmailAndPasswordAsync(string email, string password)
        {
            User? user = await userRepository.GetUserByEmailAsync(email);

            if (user == null || user.IsDisabled || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null;
            }

            user.LastLogin = DateTime.UtcNow;

            await userRepository.UpdateUserAsync(user);

            return user;
        }
    }
}
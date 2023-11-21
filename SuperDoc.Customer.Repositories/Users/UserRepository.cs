using Microsoft.EntityFrameworkCore;
using SuperDoc.Customer.Repositories.Contexts;
using SuperDoc.Customer.Repositories.Entities.Users;

namespace SuperDoc.Customer.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly SuperDocContext superDocContext;

        public UserRepository(SuperDocContext superDocContext)
        {
            this.superDocContext = superDocContext;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await superDocContext.Users.FirstOrDefaultAsync(x => x.EmailAddress == email);
        }

        public async Task AddUserAsync(User user)
        {
            await superDocContext.Users.AddAsync(user);
            await superDocContext.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            superDocContext.Users.Update(user);
            await superDocContext.SaveChangesAsync();
        }
    }
}

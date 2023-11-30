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

        public async Task AddUsersAsync(IEnumerable<User> users)
        {
            await superDocContext.Users.AddRangeAsync(users);
            await superDocContext.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            superDocContext.Users.Update(user);
            await superDocContext.SaveChangesAsync();
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await superDocContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<User?> GetUserByIdWithDocumentsAsync(Guid userId)
        {
            return await superDocContext.Users.Include(x => x.Documents).FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<IEnumerable<User>> GetUsersByEmailsAsync(string[] emails)
        {
            return await superDocContext.Users.Where(x => emails.Contains(x.EmailAddress)).ToListAsync();
        }

        public async Task<IEnumerable<User>> GetCaseManagersByIds(IEnumerable<Guid> caseManagerIds)
        {
            return await superDocContext.Users.Where(x => x.Role != Roles.User && caseManagerIds.Contains(x.UserId)).ToListAsync();
        }
    }
}

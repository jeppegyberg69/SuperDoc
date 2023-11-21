using SuperDoc.Customer.Repositories.Entities.Users;

namespace SuperDoc.Customer.Repositories.Users
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string email);
        Task UpdateUserAsync(User user);
    }
}
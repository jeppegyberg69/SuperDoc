using SuperDoc.Customer.Repositories.Entities.Users;

namespace SuperDoc.Customer.Repositories.Users
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task AddUsersAsync(IEnumerable<User> users);
        Task<IEnumerable<User>> GetCaseManagersByIds(IEnumerable<Guid> caseManagerIds);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(Guid userId);
        Task<User?> GetUserByIdWithDocumentsAsync(Guid userId);
        Task<IEnumerable<User>> GetUsersByEmailsAsync(string[] emails);
        Task UpdateUserAsync(User user);
    }
}
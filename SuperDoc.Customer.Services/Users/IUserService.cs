using SuperDoc.Customer.Repositories.Entities.Users;

namespace SuperDoc.Customer.Services.Users
{
    public interface IUserService
    {
        /// <summary>
        ///     Add a new user to the database
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="role"></param>
        /// <returns>
        ///     0 = The user was added successfully<br></br>
        ///     1 = The user already exists
        /// </returns>
        Task<int> AddUserAsync(string firstName, string lastName, string email, int? phoneCode, long? phoneNumber, string password, Roles role);

        /// <summary>
        ///     Gets a user by email and password. 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>
        ///     Returns null if the user does not exist, the password is incorrect, or the user account is disabled.
        /// </returns>
        Task<User?> GetUserByEmailAndPasswordAsync(string email, string password);
    }
}
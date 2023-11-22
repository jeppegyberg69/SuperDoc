using SuperDoc.Customer.Repositories.Entities.Users;

namespace SuperDoc.Customer.Services.Cases
{
    public interface ICaseService
    {
        Task<IEnumerable<User>> GetAllCaseManagersAsync(Guid? caseId = null);
    }
}
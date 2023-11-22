using SuperDoc.Customer.Repositories.Entities.Users;

namespace SuperDoc.Customer.Repositories.Cases
{
    public interface ICaseRepository
    {
        Task<IEnumerable<User>> GetAllCaseManagersAsync(Guid? caseId = null);
    }
}
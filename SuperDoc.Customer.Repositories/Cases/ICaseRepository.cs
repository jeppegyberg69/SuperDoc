using SuperDoc.Customer.Repositories.Entities.Cases;
using SuperDoc.Customer.Repositories.Entities.Users;

namespace SuperDoc.Customer.Repositories.Cases
{
    public interface ICaseRepository
    {
        Task CreateCase(Case docCase);
        Task<IEnumerable<User>> GetAllCaseManagersAsync(Guid? caseId = null);
        Task<IEnumerable<Case>> GetAllCasesAUserIsAssignedWithResponsibleUserAndCaseManagers(Guid userId);
        Task<IEnumerable<Case>> GetAllCasesWithResponsibleUserAndCaseManagersAsync();
        Case? GetCaseByExternalUserIdAndCaseId(Guid externalUserId, Guid caseId);
        Task<Case?> GetCaseByIdAsync(Guid caseId);
        Task<Case?> GetCaseByIdWithCaseManagersAsync(Guid caseId);
        Task UpdateCase(Case docCase);
    }
}
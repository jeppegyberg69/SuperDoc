using SuperDoc.Customer.Repositories.Entities.Cases;
using SuperDoc.Customer.Repositories.Entities.Users;

namespace SuperDoc.Customer.Repositories.Cases
{
    public interface ICaseRepository
    {
        Task CreateCase(Case docCase);
        Task<IEnumerable<User>> GetAllCaseManagersAsync(Guid? caseId = null);
        Task<IEnumerable<Case>> GetAllCasesAUserIsAssignedToWithResponsibleUserAndCaseManagers(Guid userId);
        Task<IEnumerable<Case>> GetAllCasesWithResponsibleUserAndCaseManagersAsync();
        Case? GetCaseByExternalUserIdAndCaseId(Guid externalUserId, Guid caseId);
        Task<Case?> GetCaseByIdAsync(Guid caseId);
        Task<Case?> GetCaseByIdWithCaseManagersAndResponsibleUserAsync(Guid caseId);
        Task<Case?> GetCaseByIdWithCaseManagersAsync(Guid caseId);
        Task<IEnumerable<Case>> GetCasesByIdsWithResponsibleUserAndCaseManagersAsync(IEnumerable<Guid> caseIds);
        Task UpdateCaseAsync(Case docCase);
    }
}
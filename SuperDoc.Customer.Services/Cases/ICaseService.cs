using SuperDoc.Customer.Repositories.Entities.Cases;
using SuperDoc.Customer.Repositories.Entities.Users;
using SuperDoc.Shared.Models.Cases;

namespace SuperDoc.Customer.Services.Cases
{
    public interface ICaseService
    {
        /// <summary>
        ///     This method will create or update a case.
        /// </summary>
        /// <param name="docCase"></param>
        /// <returns>
        ///     null: Success<br></br>
        ///     not null: Error message
        /// </returns>
        Task<string?> CreateOrUpdateCaseAsync(CreateOrUpdateCaseDto docCase);
        Task<IEnumerable<User>> GetAllCaseManagersAsync(Guid? caseId = null);

        /// <summary>
        ///     Get all cases a user is assigned to. Set userId to null to get all cases in the system.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<Case>> GetAssignedCasesAsync(Guid? userId);
    }
}
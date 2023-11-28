using SuperDoc.Customer.Repositories.Entities.Cases;
using SuperDoc.Customer.Repositories.Entities.Users;
using SuperDoc.Customer.Services.Cases.StatusModels;
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
        ///     Case: The case created or updated<br></br>
        ///     Case null: Error message
        /// </returns>
        Task<CreateOrUpdateCaseStatusModel> CreateOrUpdateCaseAsync(CreateOrUpdateCaseDto docCase);
        Task<IEnumerable<User>> GetAllCaseManagersAsync(Guid? caseId = null);

        /// <summary>
        ///     Get all the cases a user has access to.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<Case>> GetAssignedCasesAsync(Guid userId);
    }
}
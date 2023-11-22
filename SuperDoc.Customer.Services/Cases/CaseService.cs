using SuperDoc.Customer.Repositories.Cases;
using SuperDoc.Customer.Repositories.Entities.Users;

namespace SuperDoc.Customer.Services.Cases
{
    public class CaseService : ICaseService
    {
        private readonly ICaseRepository caseRepository;

        public CaseService(ICaseRepository caseRepository)
        {
            this.caseRepository = caseRepository;
        }

        public async Task<IEnumerable<User>> GetAllCaseManagersAsync(Guid? caseId = null)
        {
            return await caseRepository.GetAllCaseManagersAsync(caseId);
        }
    }
}

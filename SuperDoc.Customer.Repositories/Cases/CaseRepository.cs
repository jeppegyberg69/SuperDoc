using Microsoft.EntityFrameworkCore;
using SuperDoc.Customer.Repositories.Contexts;
using SuperDoc.Customer.Repositories.Entities.Cases;
using SuperDoc.Customer.Repositories.Entities.Users;

namespace SuperDoc.Customer.Repositories.Cases
{
    public class CaseRepository : ICaseRepository
    {
        private readonly SuperDocContext superDocContext;

        public CaseRepository(SuperDocContext superDocContext)
        {
            this.superDocContext = superDocContext;
        }

        public async Task<Case?> GetCaseByIdWithCaseManagersAsync(Guid caseId)
        {
            return await superDocContext.Cases.Include(x => x.CaseManagers).FirstOrDefaultAsync(x => x.CaseId == caseId);
        }

        public async Task<IEnumerable<User>> GetAllCaseManagersAsync(Guid? caseId = null)
        {
            if (caseId.HasValue)
            {
                return (await superDocContext.Cases.Include(x => x.CaseManagers).FirstOrDefaultAsync(x => x.CaseId == caseId.Value))?.CaseManagers ?? new List<User>();
            }

            return await superDocContext.Users.Where(x => x.Role != Roles.User).ToListAsync();
        }

        public async Task CreateCase(Case docCase)
        {
            await superDocContext.Cases.AddAsync(docCase);
            await superDocContext.SaveChangesAsync();
        }

        public async Task UpdateCase(Case docCase)
        {
            superDocContext.Cases.Update(docCase);
            await superDocContext.SaveChangesAsync();
        }

    }
}

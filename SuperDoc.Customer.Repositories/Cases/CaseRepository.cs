using Microsoft.EntityFrameworkCore;
using SuperDoc.Customer.Repositories.Contexts;
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


        public async Task<IEnumerable<User>> GetAllCaseManagersAsync(Guid? caseId = null)
        {
            if (caseId.HasValue)
            {
                return (await superDocContext.Cases.Include(x => x.CaseManagers).FirstOrDefaultAsync(x => x.CaseId == caseId.Value))?.CaseManagers ?? new List<User>();
            }

            return await superDocContext.Users.Where(x => x.Role != Roles.User).ToListAsync();
        }

    }
}

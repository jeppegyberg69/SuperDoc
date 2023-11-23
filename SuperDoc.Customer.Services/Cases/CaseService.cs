using SuperDoc.Customer.Repositories.Cases;
using SuperDoc.Customer.Repositories.Entities.Cases;
using SuperDoc.Customer.Repositories.Entities.Users;
using SuperDoc.Customer.Repositories.Users;
using SuperDoc.Shared.Models.Cases;

namespace SuperDoc.Customer.Services.Cases
{
    public class CaseService : ICaseService
    {
        private readonly ICaseRepository caseRepository;
        private readonly IUserRepository userRepository;

        public CaseService(ICaseRepository caseRepository, IUserRepository userRepository)
        {
            this.caseRepository = caseRepository;
            this.userRepository = userRepository;
        }


        public async Task<IEnumerable<Case>> GetAssignedCasesAsync(Guid? userId)
        {
            if (!userId.HasValue)
            {
                return await caseRepository.GetAllCasesWithResponsibleUserAndCaseManagersAsync();
            }

            return await caseRepository.GetAllCasesAUserIsAssignedWithResponsibleUserAndCaseManagers(userId.Value);
        }

        public async Task<IEnumerable<User>> GetAllCaseManagersAsync(Guid? caseId = null)
        {
            return await caseRepository.GetAllCaseManagersAsync(caseId);
        }

        public async Task<string?> CreateOrUpdateCaseAsync(CreateOrUpdateCaseDto docCase)
        {

            User? responsibleUser = await userRepository.GetUserByIdAsync(docCase.ResponsibleUserId);


            if (responsibleUser == null || responsibleUser?.Role == Roles.User)
            {
                return "Invalid responsibleUserId";
            }

            var caseManagers = await userRepository.GetCaseManagersByIds(docCase.CaseMangers);


            foreach (var caseManagerId in docCase.CaseMangers)
            {
                if (!caseManagers.Any(x => x.UserId == caseManagerId))
                {
                    return "Invalid caseManagerId: " + caseManagerId.ToString();
                }
            }

            if (docCase.CaseId.HasValue)
            {
                var dbcase = await caseRepository.GetCaseByIdWithCaseManagersAsync(docCase.CaseId.Value);

                if (dbcase == null)
                {
                    return "Invalid caseId";
                }

                dbcase.CaseManagers?.Clear();

                dbcase.Title = docCase.Title;
                dbcase.Description = docCase.Description;
                dbcase.ResponsibleUser = responsibleUser;
                dbcase.CaseManagers = caseManagers.ToList();
                dbcase.DateModified = DateTime.UtcNow;

                await caseRepository.UpdateCase(dbcase);
            }
            else
            {
                Case newCase = new Case()
                {
                    Title = docCase.Title,
                    Description = docCase.Description,
                    ResponsibleUser = responsibleUser,
                    CaseManagers = caseManagers.ToList(),
                    DateModified = DateTime.UtcNow,
                    DateCreated = DateTime.UtcNow,
                    IsArchived = false
                };

                await caseRepository.CreateCase(newCase);
            }

            return null;
        }
    }
}

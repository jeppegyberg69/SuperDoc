using SuperDoc.Customer.Repositories.Cases;
using SuperDoc.Customer.Repositories.Entities.Cases;
using SuperDoc.Customer.Repositories.Entities.Users;
using SuperDoc.Customer.Repositories.Users;
using SuperDoc.Customer.Services.Shared.StatusModels;
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


        public async Task<IEnumerable<Case>> GetAssignedCasesAsync(Guid userId)
        {
            User? user = await userRepository.GetUserByIdWithDocumentsAsync(userId);

            if (user == null)
            {
                return new List<Case>();
            }

            if (user.Role == Roles.SuperAdmin)
            {
                return await caseRepository.GetAllCasesWithResponsibleUserAndCaseManagersAsync();
            }
            else if (user.Role == Roles.Admin || user.Role == Roles.CaseManager)
            {
                return await caseRepository.GetAllCasesAUserIsAssignedToWithResponsibleUserAndCaseManagers(user.UserId);
            }
            else
            {
#nullable disable
                List<Guid> caseIds = user.Documents?.Where(x => x.CaseId.HasValue).DistinctBy(x => x.CaseId).Select(x => x.CaseId.Value).ToList() ?? new List<Guid>();
#nullable enable

                return await caseRepository.GetCasesByIdsWithResponsibleUserAndCaseManagersAsync(caseIds);
            }
        }

        public async Task<IEnumerable<User>> GetAllCaseManagersAsync(Guid? caseId = null)
        {
            return await caseRepository.GetAllCaseManagersAsync(caseId);
        }

        public async Task<ResultModel<Case>> CreateOrUpdateCaseAsync(CreateOrUpdateCaseDto docCase)
        {

            User? responsibleUser = await userRepository.GetUserByIdAsync(docCase.ResponsibleUserId);


            if (responsibleUser == null || responsibleUser?.Role == Roles.User)
            {
                return new ResultModel<Case>("Invalid responsibleUserId");
            }

            var caseManagers = await userRepository.GetCaseManagersByIds(docCase.CaseMangers);


            foreach (var caseManagerId in docCase.CaseMangers)
            {
                if (!caseManagers.Any(x => x.UserId == caseManagerId))
                {
                    return new ResultModel<Case>("Invalid caseManagerId: " + caseManagerId.ToString());
                }
            }

            if (docCase.CaseId.HasValue)
            {
                var dbcase = await caseRepository.GetCaseByIdWithCaseManagersAndResponsibleUserAsync(docCase.CaseId.Value);

                if (dbcase == null)
                {
                    return new ResultModel<Case>("Invalid caseId");
                }

                dbcase.CaseManagers?.Clear();

                dbcase.Title = docCase.Title;
                dbcase.Description = docCase.Description;
                dbcase.ResponsibleUser = responsibleUser;
                dbcase.CaseManagers = caseManagers.ToList();
                dbcase.DateModified = DateTime.UtcNow;

                await caseRepository.UpdateCase(dbcase);
                return new ResultModel<Case>(dbcase);
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
                return new ResultModel<Case>(newCase);
            }
        }
    }
}

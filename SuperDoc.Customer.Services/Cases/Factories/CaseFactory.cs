using SuperDoc.Customer.Repositories.Entities.Cases;
using SuperDoc.Customer.Repositories.Entities.Users;
using SuperDoc.Shared.Models.Cases;

namespace SuperDoc.Customer.Services.Cases.Factories
{
    public class CaseFactory : ICaseFactory
    {
        public IEnumerable<CaseManagerDto> ConverUsersToCaseManagerDtos(IEnumerable<User> users)
        {
            var managers = new List<CaseManagerDto>();

            if (users != null)
            {
                foreach (var user in users)
                {
                    managers.Add(ConvertUserToCaseManagerDto(user));
                }
            }

            return managers;
        }

        public CaseManagerDto ConvertUserToCaseManagerDto(User user)
        {
            return new CaseManagerDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.EmailAddress
            };
        }

        public IEnumerable<CaseDto> ConverCasesToDtos(IEnumerable<Case> cases)
        {
            List<CaseDto> caseDtos = new List<CaseDto>();

            if (cases != null)
            {
                foreach (var docCase in cases)
                {
                    caseDtos.Add(ConverCaseToDto(docCase));
                }
            }

            return caseDtos;
        }

        public CaseDto ConverCaseToDto(Case docCase)
        {
            return new CaseDto
            {
                CaseId = docCase.CaseId,
                CaseNumber = docCase.CaseNumber,
                Title = docCase.Title,
                Description = docCase.Description,
                ResponsibleUser = docCase.ResponsibleUser == null ? new CaseManagerDto() : ConvertUserToCaseManagerDto(docCase.ResponsibleUser),
                CaseManagers = docCase.CaseManagers != null ? ConverUsersToCaseManagerDtos(docCase.CaseManagers) : new List<CaseManagerDto>()
            };
        }
    }
}

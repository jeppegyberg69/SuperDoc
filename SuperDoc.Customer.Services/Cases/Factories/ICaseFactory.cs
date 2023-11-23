using SuperDoc.Customer.Repositories.Entities.Cases;
using SuperDoc.Customer.Repositories.Entities.Users;
using SuperDoc.Shared.Models.Cases;

namespace SuperDoc.Customer.Services.Cases.Factories
{
    public interface ICaseFactory
    {
        IEnumerable<CaseDto> ConverCasesToDtos(IEnumerable<Case> cases);
        CaseDto ConverCaseToDto(Case docCase);
        CaseManagerDto ConvertUserToCaseManagerDto(User user);
        IEnumerable<CaseManagerDto> ConverUsersToCaseManagerDtos(IEnumerable<User> users);
    }
}
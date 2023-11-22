using SuperDoc.Customer.Repositories.Entities.Users;
using SuperDoc.Shared.Models.Cases;

namespace SuperDoc.Customer.Services.Cases.Factories
{
    public interface ICaseFactory
    {
        IEnumerable<CaseManagerDto> ConverUsersToCaseManagerDtos(IEnumerable<User> users);
    }
}
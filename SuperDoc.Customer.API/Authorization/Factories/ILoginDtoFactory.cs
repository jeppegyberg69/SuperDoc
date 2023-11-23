using SuperDoc.Customer.Repositories.Entities.Users;
using SuperDoc.Shared.Models.Users;

namespace SuperDoc.Customer.API.Authorization.Factories
{
    public interface ILoginDtoFactory
    {
        TokenDto CreateTokenDto(User user, string token, DateTime validFrom, DateTime validTo);
    }
}
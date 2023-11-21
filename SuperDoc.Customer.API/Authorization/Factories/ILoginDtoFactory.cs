using SuperDoc.Shared.Models;

namespace SuperDoc.Customer.API.Authorization.Factories
{
    public interface ILoginDtoFactory
    {
        TokenDto CreateTokenDto(string token, DateTime validFrom, DateTime validTo);
    }
}
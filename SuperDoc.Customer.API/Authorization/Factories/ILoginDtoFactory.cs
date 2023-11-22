using SuperDoc.Shared.Models.Users;

namespace SuperDoc.Customer.API.Authorization.Factories
{
    public interface ILoginDtoFactory
    {
        TokenDto CreateTokenDto(string token, DateTime validFrom, DateTime validTo);
    }
}
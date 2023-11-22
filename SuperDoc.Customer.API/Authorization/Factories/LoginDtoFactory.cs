using SuperDoc.Shared.Models.Users;

namespace SuperDoc.Customer.API.Authorization.Factories
{
    public class LoginDtoFactory : ILoginDtoFactory
    {
        public TokenDto CreateTokenDto(string token, DateTime validFrom, DateTime validTo)
        {
            return new TokenDto
            {
                Token = token,
                ValidFrom = validFrom,
                ValidTo = validTo
            };
        }
    }
}

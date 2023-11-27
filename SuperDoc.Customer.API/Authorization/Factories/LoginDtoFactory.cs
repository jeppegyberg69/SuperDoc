using SuperDoc.Customer.Repositories.Entities.Users;
using SuperDoc.Shared.Models.Users;

namespace SuperDoc.Customer.API.Authorization.Factories
{
    public class LoginDtoFactory : ILoginDtoFactory
    {
        public TokenDto CreateTokenDto(User user, string token, DateTime validFrom, DateTime validTo)
        {
            return new TokenDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.EmailAddress,
                PhoneCode = user.PhoneCode,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role.ToString(),
                Token = token,
                ValidFrom = validFrom,
                ValidTo = validTo
            };
        }
    }
}

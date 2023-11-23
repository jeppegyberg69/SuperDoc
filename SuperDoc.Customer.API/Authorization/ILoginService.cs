using System.Security.Claims;
using SuperDoc.Customer.Repositories.Entities.Users;
using SuperDoc.Shared.Models.Users;

namespace SuperDoc.Customer.API.Authorization
{
    public interface ILoginService
    {
        TokenDto GenerateToken(User user);
        Guid? GetUserId(IEnumerable<Claim> claims);
        bool IsUserInRole(IEnumerable<Claim> claims, Roles role);
    }
}
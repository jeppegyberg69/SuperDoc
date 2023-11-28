using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SuperDoc.Customer.API.Authorization.Factories;
using SuperDoc.Customer.API.Authorization.Identity;
using SuperDoc.Customer.Repositories.Entities.Users;
using SuperDoc.Shared.Models.Users;

namespace SuperDoc.Customer.API.Authorization
{
    public class LoginService : ILoginService
    {
        private readonly IConfiguration configuration;
        private readonly ILoginDtoFactory loginDtoFactory;

        public LoginService(IConfiguration configuration, ILoginDtoFactory loginDtoFactory)
        {
            this.configuration = configuration;
            this.loginDtoFactory = loginDtoFactory;
        }

        public Guid GetUserId(IEnumerable<Claim> claims)
        {
#nullable disable
            return Guid.Parse(claims.FirstOrDefault(x => x.Type == IdentityData.UserIdClaimName)?.Value ?? null);
#nullable enable
        }

        public bool IsUserInRole(IEnumerable<Claim> claims, Roles role)
        {
            string userRole = claims.FirstOrDefault(x => x.Type == IdentityData.UserRoleClaimName)?.Value ?? string.Empty;

            if (!string.IsNullOrEmpty(userRole))
            {
                if (userRole == role.ToString())
                {
                    return true;
                }
            }

            return false;
        }


        public TokenDto GenerateToken(User user)
        {
            DateTime validFrom = DateTime.UtcNow;
            DateTime validTo = DateTime.UtcNow.AddHours(8);

            var tokenHandler = new JwtSecurityTokenHandler();
#nullable disable
            var key = Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]);
#nullable enable


            var claims = new List<Claim>
            {
                new Claim(IdentityData.UserIdClaimName, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.EmailAddress),
                new Claim(IdentityData.UserRoleClaimName, user.Role.ToString()),
            };


            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                NotBefore = validFrom,
                Expires = validTo,
                Issuer = configuration["JwtSettings:Issuer"],
                Audience = configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescription);

            return loginDtoFactory.CreateTokenDto(user, tokenHandler.WriteToken(token), validFrom, validTo);
        }
    }
}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SuperDoc.Customer.API.Authorization.Factories;
using SuperDoc.Customer.API.Authorization.Identity;
using SuperDoc.Customer.Repositories.Entities.Users;
using SuperDoc.Shared.Models;

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


        public TokenDto GenerateToken(User user, DateTime validFrom, DateTime validTo)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]);


            var claims = new List<Claim>
            {
                new Claim("userId", user.UserId.ToString()),
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

            return loginDtoFactory.CreateTokenDto(tokenHandler.WriteToken(token), validFrom, validTo);
        }
    }
}

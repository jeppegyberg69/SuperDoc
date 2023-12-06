using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SuperDoc.Customer.Repositories.Entities.Users;

namespace SuperDoc.Customer.API.Services.Authorization.Identity
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequiredRoleAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IEnumerable<Roles> claimValues;

        public RequiredRoleAttribute(params Roles[] values)
        {
            claimValues = values;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {

            if (context.HttpContext.User.Identity?.IsAuthenticated ?? false)
            {
                if (!claimValues.Any(x => context.HttpContext.User.HasClaim(IdentityData.UserRoleClaimName, x.ToString())))
                {
                    context.Result = new ForbidResult();
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
            }


        }
    }
}

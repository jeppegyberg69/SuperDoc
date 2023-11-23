using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperDoc.Customer.API.Authorization;
using SuperDoc.Customer.API.Authorization.Identity;
using SuperDoc.Customer.Repositories.Entities.Users;
using SuperDoc.Customer.Services.Cases;
using SuperDoc.Customer.Services.Cases.Factories;
using SuperDoc.Shared.Models.Cases;

namespace SuperDoc.Customer.API.Controllers
{
    [Authorize]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class CaseController : ControllerBase
    {
        private readonly ICaseService caseService;
        private readonly ICaseFactory caseFactory;
        private readonly ILoginService loginService;

        public CaseController(ICaseService caseService, ICaseFactory caseFactory, ILoginService loginService)
        {
            this.caseService = caseService;
            this.caseFactory = caseFactory;
            this.loginService = loginService;
        }

        [HttpGet]
        [RequiredRole(Roles.CaseManager, Roles.Admin, Roles.SuperAdmin)]
        public async Task<ActionResult<IEnumerable<CaseManagerDto>>> GetCaseManagers(Guid? caseId)
        {
            var caseManagers = await caseService.GetAllCaseManagersAsync(caseId);

            return Ok(caseFactory.ConverUsersToCaseManagerDtos(caseManagers));
        }

        [HttpPost]
        [RequiredRole(Roles.CaseManager, Roles.Admin, Roles.SuperAdmin)]
        public async Task<IActionResult> CreateOrUpdateCase([FromBody] CreateOrUpdateCaseDto docCase)
        {
            if (!(docCase.CaseMangers?.Any() ?? false))
            {
                return BadRequest("The must be at least 1 case manager");
            }

            var errorMessage = await caseService.CreateOrUpdateCaseAsync(docCase);

            if (errorMessage != null)
            {
                return BadRequest(errorMessage);
            }

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetCases()
        {
            Guid? userId = loginService.GetUserId(User.Claims);

            if (userId == null)
            {
                return BadRequest("User id is null");
            }

            if (loginService.IsUserInRole(User.Claims, Roles.SuperAdmin))
            {
                userId = null;
            }

            var cases = await caseService.GetAssignedCasesAsync(userId);

            return Ok(caseFactory.ConverCasesToDtos(cases));
        }
    }
}

﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public CaseController(ICaseService caseService, ICaseFactory caseFactory)
        {
            this.caseService = caseService;
            this.caseFactory = caseFactory;
        }

        [RequiredRole(Roles.CaseManager, Roles.Admin, Roles.SuperAdmin)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CaseManagerDto>>> GetCaseManagers(Guid? caseId)
        {
            var caseManagers = await caseService.GetAllCaseManagersAsync(caseId);

            return Ok(caseFactory.ConverUsersToCaseManagerDtos(caseManagers));
        }
    }
}

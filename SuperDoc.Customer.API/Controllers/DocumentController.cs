using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperDoc.Customer.API.Authorization;
using SuperDoc.Customer.API.Authorization.Identity;
using SuperDoc.Customer.Repositories.Entities.Users;
using SuperDoc.Customer.Services.Documents;
using SuperDoc.Customer.Services.Security;
using SuperDoc.Shared.Models.Documents;

namespace SuperDoc.Customer.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService documentService;
        private readonly IAccessService accessService;
        private readonly ILoginService loginService;

        public DocumentController(IDocumentService documentService, IAccessService accessService, ILoginService loginService)
        {
            this.documentService = documentService;
            this.accessService = accessService;
            this.loginService = loginService;
        }

        [HttpPost]
        [RequiredRole(Roles.SuperAdmin, Roles.Admin, Roles.CaseManager)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Guid>> CreateOrUpdateDocument([FromBody] CreateOrUpdateDocumentDto documentDto)
        {
            bool? accessResult = null;

            if (documentDto.DocumentId.HasValue)
            {
                accessResult = await accessService.HasAccessToDocumentAsync(documentDto.DocumentId.Value, loginService.GetUserId(User.Claims));
            }
            else if (documentDto.CaseId.HasValue)
            {
                accessResult = await accessService.HasAccessToCaseAsync(documentDto.CaseId.Value, loginService.GetUserId(User.Claims));
            }
            else
            {
                return BadRequest("CaseId or DocumentId must be set");
            }


            if (!accessResult.HasValue)
            {
                return NotFound();
            }
            else if (!accessResult.Value)
            {
                return Forbid();
            }


            Guid? documentId = await documentService.CreateOrUpdateDocumentAsync(documentDto);

            if (!documentId.HasValue)
            {
                return NotFound();
            }

            return Ok(documentId.Value);
        }
    }
}

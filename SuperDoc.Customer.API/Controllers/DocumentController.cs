using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperDoc.Customer.API.Authorization;
using SuperDoc.Customer.API.Authorization.Identity;
using SuperDoc.Customer.Repositories.Entities.Documents;
using SuperDoc.Customer.Repositories.Entities.Users;
using SuperDoc.Customer.Services.Documents;
using SuperDoc.Customer.Services.Documents.Factories;
using SuperDoc.Customer.Services.Revisions.Factories;
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
        private readonly IRevisionFactory revisionFactory;
        private readonly IDocumentFactory documentFactory;

        public DocumentController(IDocumentService documentService, IAccessService accessService, ILoginService loginService, IRevisionFactory revisionFactory, IDocumentFactory documentFactory)
        {
            this.documentService = documentService;
            this.accessService = accessService;
            this.loginService = loginService;
            this.revisionFactory = revisionFactory;
            this.documentFactory = documentFactory;
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


        [HttpPost]
        [RequiredRole(Roles.SuperAdmin, Roles.Admin, Roles.CaseManager)]
        public async Task<IActionResult> UploadDocument(Guid documentId, string emailAddresses, IFormFile documentFile)
        {
            bool? accessResult = await accessService.HasAccessToDocumentAsync(documentId, loginService.GetUserId(User.Claims));

            if (accessResult == null)
            {
                return BadRequest("Invalid documentId");
            }

            if (!accessResult.Value)
            {
                return Forbid();
            }


            if (!documentFile.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Only PDF files are allowed");
            }

            var result = await documentService.SaveUploadedFile(loginService.GetUserId(User.Claims), documentId, emailAddresses, documentFile);

            if (result.Result == null)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(revisionFactory.ConvertRevisionToDto(result.Result));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocumentDto>>> GetDocumentsByCaseId(Guid caseId)
        {
            bool? accessResult = await accessService.HasAccessToCaseAsync(caseId, loginService.GetUserId(User.Claims));

            if (accessResult == null)
            {
                return BadRequest("Invalid caseId");
            }

            if (!accessResult.Value)
            {
                return Forbid();
            }

            IEnumerable<Document> documents = await documentService.GetDocumentsByCaseIdAndUserIdWithExternalUsersAsync(caseId, loginService.GetUserId(User.Claims));

            return Ok(documentFactory.CreateDocumentDtos(documents));
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperDoc.Customer.API.Authorization;
using SuperDoc.Customer.API.Authorization.Identity;
using SuperDoc.Customer.Repositories.Entities.Documents;
using SuperDoc.Customer.Repositories.Entities.Users;
using SuperDoc.Customer.Services.Revisions;
using SuperDoc.Customer.Services.Revisions.Factories;
using SuperDoc.Customer.Services.Security;
using SuperDoc.Customer.Services.Shared.StatusModels;
using SuperDoc.Shared.Models.Documents;
using SuperDoc.Shared.Models.Revisions;

namespace SuperDoc.Customer.API.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class RevisionController : ControllerBase
    {
        private readonly IRevisionService revisionService;
        private readonly IAccessService accessService;
        private readonly ILoginService loginService;
        private readonly IRevisionFactory revisionFactory;

        public RevisionController(IRevisionService revisionService, IAccessService accessService, ILoginService loginService, IRevisionFactory revisionFactory)
        {
            this.revisionService = revisionService;
            this.accessService = accessService;
            this.loginService = loginService;
            this.revisionFactory = revisionFactory;
        }

        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(IEnumerable<RevisionDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RevisionDto>>> GetRevisionsByDocumentId(Guid documentId)
        {
            bool? accessResult = await accessService.HasAccessToDocumentAsync(documentId, loginService.GetUserId(User.Claims));

            if (accessResult == null)
            {
                return NotFound("Invalid documentId");
            }
            else if (accessResult == false)
            {
                return Forbid();
            }

            var revisions = await revisionService.GetRevisionsByDocumentIdWithDocumentSignaturesAndUsersAsync(documentId);

            return Ok(revisionFactory.ConvertRevisionsToDtos(revisions));
        }

        [HttpPost]
        [RequiredRole(Roles.SuperAdmin, Roles.Admin, Roles.CaseManager)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(RevisionDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<RevisionDto>> UploadRevision(Guid documentId, string emailAddresses, IFormFile documentFile)
        {
            bool? accessResult = await accessService.HasAccessToDocumentAsync(documentId, loginService.GetUserId(User.Claims));

            if (accessResult == null)
            {
                return NotFound("Invalid documentId");
            }
            else if (!accessResult.Value)
            {
                return Forbid();
            }


            if (!documentFile.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Only PDF files are allowed");
            }

            var result = await revisionService.SaveRevisionAsync(loginService.GetUserId(User.Claims), documentId, emailAddresses, documentFile);

            if (result.Result == null)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(revisionFactory.ConvertRevisionToDto(result.Result));
        }

        [HttpGet]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]

        public async Task<IActionResult> DownloadRevision(Guid revisionId)
        {
            bool? accessResult = await accessService.HasAccessToRevision(revisionId, loginService.GetUserId(User.Claims));

            if (accessResult == null)
            {
                return NotFound("Invalid revisionId");
            }
            else if (!accessResult.Value)
            {
                return Forbid();
            }


            Revision? revision = await revisionService.GetRevisionByIdAsync(revisionId);

            if (revision == null)
            {
                return NotFound("Invalid revisionId");
            }

            if (!System.IO.File.Exists(revision.FilePath))
            {
                return NotFound("File not found");
            }

            return new FileStreamResult(System.IO.File.OpenRead(revision.FilePath), "application/pdf");
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DocumentSignatureDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<DocumentSignatureDto>> SignRevision([FromBody] RevisionSignDto signatureDto)
        {
            bool? accessResult = await accessService.HasAccessToRevision(signatureDto.RevisionId, loginService.GetUserId(User.Claims));

            if (accessResult == null)
            {
                return NotFound("Invalid revisionId");
            }
            else if (!accessResult.Value)
            {
                return Forbid();
            }

            ResultModel<DocumentSignature> result = await revisionService.SignRevisionAsync(signatureDto.RevisionId, loginService.GetUserId(User.Claims), signatureDto.Signature, signatureDto.PublicKey);

            if (result.Result == null)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(revisionFactory.ConvertSignatureToDto(result.Result));
        }

    }
}

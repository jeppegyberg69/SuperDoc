using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperDoc.Customer.API.Authorization;
using SuperDoc.Customer.Services.Revisions;
using SuperDoc.Customer.Services.Revisions.Factories;
using SuperDoc.Customer.Services.Security;
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

            if (accessResult == false)
            {
                return Forbid();
            }

            var revisions = await revisionService.GetRevisionsByDocumentIdWithDocumentSignaturesAsync(documentId);

            return Ok(revisionFactory.ConvertRevisionsToDtos(revisions));
        }
    }
}

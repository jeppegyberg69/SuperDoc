using SuperDoc.Customer.Repositories.Cases;
using SuperDoc.Customer.Repositories.Documents;
using SuperDoc.Customer.Repositories.Entities.Cases;
using SuperDoc.Customer.Repositories.Entities.Documents;
using SuperDoc.Customer.Repositories.Entities.Users;
using SuperDoc.Customer.Repositories.Revisions;
using SuperDoc.Customer.Repositories.Users;

namespace SuperDoc.Customer.Services.Security
{
    public class AccessService : IAccessService
    {
        private readonly IDocumentRepository documentRepository;
        private readonly IUserRepository userRepository;
        private readonly ICaseRepository caseRepository;
        private readonly IRevisionRepository revisionRepository;

        public AccessService(IDocumentRepository documentRepository, IUserRepository userRepository, ICaseRepository caseRepository, IRevisionRepository revisionRepository)
        {
            this.documentRepository = documentRepository;
            this.userRepository = userRepository;
            this.caseRepository = caseRepository;
            this.revisionRepository = revisionRepository;
        }


        public async Task<bool?> HasAccessToRevision(Guid revisionId, Guid userId)
        {
            User? user = await userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            Revision? revision = await revisionRepository.GetRevisionByIdAsync(revisionId);

            if (revision == null)
            {
                return null;
            }

            Guid documentId = revision.DocumentId.HasValue ? revision.DocumentId.Value : Guid.Empty;

            if (user.Role == Roles.SuperAdmin)
            {
                return true;
            }
            else if (user.Role == Roles.Admin || user.Role == Roles.CaseManager)
            {
                Document? document = await documentRepository.GetDocumentByIdAsync(documentId);

                if (document?.CaseId == null)
                {
                    return null;
                }

                Case? docCase = await caseRepository.GetCaseByIdWithCaseManagersAsync(document.CaseId.Value);

                if (docCase == null)
                {
                    return null;
                }

                return docCase.CaseManagers?.Any(x => x.UserId == userId) ?? false;
            }
            else
            {
                Document? document = await documentRepository.GetDocumentByIdWithExternialUsersAsync(documentId);

                if (document == null)
                {
                    return null;
                }

                return document.ExternalUsers?.Any(x => x.UserId == userId) ?? false;
            }
        }

        public async Task<bool?> HasAccessToDocumentAsync(Guid documentId, Guid userId)
        {
            User? user = await userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            if (user.Role == Roles.SuperAdmin)
            {
                return true;
            }
            else if (user.Role == Roles.Admin || user.Role == Roles.CaseManager)
            {
                Document? document = await documentRepository.GetDocumentByIdAsync(documentId);

                if (document?.CaseId == null)
                {
                    return null;
                }

                Case? docCase = await caseRepository.GetCaseByIdWithCaseManagersAsync(document.CaseId.Value);

                if (docCase == null)
                {
                    return null;
                }

                return docCase.CaseManagers?.Any(x => x.UserId == userId) ?? false;
            }
            else
            {
                Document? document = await documentRepository.GetDocumentByIdWithExternialUsersAsync(documentId);

                if (document == null)
                {
                    return null;
                }

                return document.ExternalUsers?.Any(x => x.UserId == userId) ?? false;
            }

        }


        public async Task<bool?> HasAccessToCaseAsync(Guid caseId, Guid userId)
        {
            User? user = await userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            if (user.Role == Roles.SuperAdmin)
            {
                return true;
            }
            else if (user.Role == Roles.Admin || user.Role == Roles.CaseManager)
            {
                Case? docCase = await caseRepository.GetCaseByIdWithCaseManagersAsync(caseId);

                if (docCase == null)
                {
                    return null;
                }

                return docCase.CaseManagers?.Any(x => x.UserId == userId) ?? false;
            }
            else
            {
                Case? docCase = caseRepository.GetCaseByExternalUserIdAndCaseId(userId, caseId);

                if (docCase == null)
                {
                    return false;
                }

                return true;
            }
        }
    }
}

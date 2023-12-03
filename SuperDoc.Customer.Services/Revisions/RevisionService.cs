using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using SuperDoc.Customer.Repositories.Cases;
using SuperDoc.Customer.Repositories.Documents;
using SuperDoc.Customer.Repositories.DocumentSignatures;
using SuperDoc.Customer.Repositories.Entities.Cases;
using SuperDoc.Customer.Repositories.Entities.Documents;
using SuperDoc.Customer.Repositories.Entities.Users;
using SuperDoc.Customer.Repositories.Revisions;
using SuperDoc.Customer.Repositories.Users;
using SuperDoc.Customer.Services.Shared.StatusModels;
using SuperDoc.Shared.Helpers;

namespace SuperDoc.Customer.Services.Revisions
{
    public class RevisionService : IRevisionService
    {
        private readonly IRevisionRepository revisionRepository;
        private readonly IUserRepository userRepository;
        private readonly IDocumentRepository documentRepository;
        private readonly IDocumentSignatureRepository documentSignatureRepository;
        private readonly ICaseRepository caseRepository;

        public RevisionService(IRevisionRepository revisionRepository, IUserRepository userRepository, IDocumentRepository documentRepository, IDocumentSignatureRepository documentSignatureRepository, ICaseRepository caseRepository)
        {
            this.revisionRepository = revisionRepository;
            this.userRepository = userRepository;
            this.documentRepository = documentRepository;
            this.documentSignatureRepository = documentSignatureRepository;
            this.caseRepository = caseRepository;
        }

        public async Task<Revision?> GetRevisionByIdAsync(Guid revisionId)
        {
            return await revisionRepository.GetRevisionByIdAsync(revisionId);
        }

        public async Task<IEnumerable<Revision>> GetRevisionsByDocumentIdWithDocumentSignaturesAndUsersAsync(Guid documentId)
        {
            return await revisionRepository.GetRevisionsByDocumentIdWithDocumentSignaturesAndUsersAsync(documentId);
        }


        public async Task<ResultModel<DocumentSignature>> SignRevisionAsync(Guid revisionId, Guid userId, string signature, string publicKey)
        {
            User? user = await userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return new ResultModel<DocumentSignature>("Invalid userId");
            }

            Revision? revision = await revisionRepository.GetRevisionByIdWithDocumentSignaturesAndUsersAsync(revisionId);

            if (revision == null)
            {
                return new ResultModel<DocumentSignature>("Invalid revisionId");
            }

            DocumentSignature? documentSignature = revision.DocumentSignatures?.FirstOrDefault(x => x.UserId == userId);

            if (documentSignature == null)
            {
                return new ResultModel<DocumentSignature>("The user is not one of the individuals who need to sign this revision.");
            }

            bool isSignatureValid = CryptographicHelper.IsSignatureValid(revision.DocumentHash, signature, publicKey);

            if (isSignatureValid)
            {
                return new ResultModel<DocumentSignature>("Invalid signature");
            }

            documentSignature.Signature = signature;
            documentSignature.PublicKey = publicKey;
            documentSignature.DateSigned = DateTime.UtcNow;

            await documentSignatureRepository.UpdateDocumentSignatureAsync(documentSignature);


            return new ResultModel<DocumentSignature>(documentSignature);
        }

        public async Task<ResultModel<Revision>> SaveRevisionAsync(Guid userId, Guid documentId, string emailAddresses, IFormFile documentFile)
        {
            User? user = await userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return new ResultModel<Revision>("Invalid user id");
            }

            if (!IsValidEmailAddresses(emailAddresses))
            {
                return new ResultModel<Revision>("The email addresses are invalid");
            }

            Document? document = await documentRepository.GetDocumentByIdWithExternialUsersAsync(documentId);

            if (document == null)
            {
                return new ResultModel<Revision>("Invalid documentId");
            }

            string? filePath = await SaveFileAsync(documentFile, documentId);

            if (string.IsNullOrEmpty(filePath))
            {
                return new ResultModel<Revision>("Unable to save file");
            }

            Revision revision = new Revision
            {
                DateUploaded = DateTime.UtcNow,
                FilePath = filePath,
                DocumentId = document.DocumentId,
                Document = document,
                UserId = user.UserId,
                DocumentHash = await CalculateHashAsync(documentFile)
            };

            await revisionRepository.CreateRevisionAsync(revision);

            Case? docCase = await caseRepository.GetCaseByIdWithCaseManagersAsync(document.CaseId ?? Guid.Empty);

            if (docCase == null)
            {
                return new ResultModel<Revision>("Invalid caseId");
            }

            IEnumerable<DocumentSignature> documentSignatures = await AddSignatureUsers(emailAddresses, revision.RevisionId, document, docCase);

            revision.DocumentSignatures = documentSignatures.ToList();

            return new ResultModel<Revision>(revision);
        }

        private async Task<IEnumerable<DocumentSignature>> AddSignatureUsers(string emailAddresses, Guid revisionId, Document document, Case docCase)
        {
            List<User> users = (await userRepository.GetUsersByEmailsAsync(emailAddresses.Split(';'))).ToList();

            #region CaseManagers
            bool updateCase = false;
            HashSet<Guid>? caseManagers = docCase.CaseManagers?.Select(x => x.UserId).ToHashSet();

            foreach (var user in users.Where(r => r.Role != Roles.User))
            {
                if (!(caseManagers?.Contains(user.UserId) ?? false))
                {
                    docCase.CaseManagers?.Add(user);
                    updateCase = true;
                }
            }
            if (updateCase)
            {
                await caseRepository.UpdateCaseAsync(docCase);
            }
            #endregion

            #region ExternalUsers
            bool updateDocument = false;
            HashSet<Guid>? externialUserIds = document.ExternalUsers?.Select(x => x.UserId).ToHashSet();

            foreach (var user in users.Where(x => x.Role == Roles.User))
            {
                if (!(externialUserIds?.Contains(user.UserId) ?? false))
                {
                    document.ExternalUsers?.Add(user);
                    updateDocument = true;
                }
            }
            if (updateDocument)
            {
                await documentRepository.UpdateDocumentAsync(document);
            }
            #endregion



            List<User> newUsers = new List<User>();

            foreach (string emailAddress in emailAddresses.Split(';'))
            {
                if (!users.Any(x => x.EmailAddress == emailAddress))
                {
                    newUsers.Add(new User
                    {
                        IsUserSignedUp = false,
                        DateModified = DateTime.UtcNow,
                        DateCreated = DateTime.UtcNow,
                        EmailAddress = emailAddress,
                        Role = Roles.User,
                        Documents = new List<Document> { document }
                    });
                }
            }

            if (newUsers.Any())
            {
                await userRepository.AddUsersAsync(newUsers);
                users.AddRange(newUsers);
            }

            List<DocumentSignature> documentSignatures = new List<DocumentSignature>();

            foreach (User signUser in users)
            {
                documentSignatures.Add(new DocumentSignature
                {
                    RevisionId = revisionId,
                    UserId = signUser.UserId,
                    DateSigned = null,
                    User = signUser
                });
            }

            await documentSignatureRepository.CreateDocumentSignaturesAsync(documentSignatures);
            return documentSignatures;
        }

        private async Task<string> CalculateHashAsync(IFormFile document)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = await sha256.ComputeHashAsync(document.OpenReadStream());

                StringBuilder hashstring = new StringBuilder(64);

                foreach (byte b in hash)
                {
                    hashstring.Append(b.ToString("X2"));
                }

                return hashstring.ToString();
            }
        }

        /// <summary>
        ///    Validates a string of email addresses
        /// </summary>
        /// <param name="emailAddresses"></param>
        /// <returns>
        ///     true if all email addresses are valid<br></br>
        ///     false if one or more email addresses are invalid
        /// </returns>
        private bool IsValidEmailAddresses(string emailAddresses)
        {
            if (string.IsNullOrWhiteSpace(emailAddresses))
            {
                return false;
            }

            string[] emailAddressesArray = emailAddresses.Split(';');

            if (!emailAddressesArray.Any())
            {
                return false;
            }

            foreach (string emailAddress in emailAddressesArray)
            {
                if (!IsValidEmailAddress(emailAddress))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///     Validates an email address
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns>
        ///     true if the email address is valid<br></br>
        ///     false if the email address is invalid
        /// </returns>
        private bool IsValidEmailAddress(string emailAddress)
        {
            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                return false;
            }

            try
            {
                // MailAddress throws an exception if the email address has invalid format.
                MailAddress m = new MailAddress(emailAddress);

                return emailAddress.Equals(m.Address, StringComparison.Ordinal);
            }
            catch (ArgumentNullException)
            {
                return false;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        /// <summary>
        ///     Saves a PDF file to the file system
        /// </summary>
        /// <param name="documentFile"></param>
        /// <param name="documentId"></param>
        /// <returns>
        ///     The file path is returned if the file is saved successfully.<br></br>
        ///     null is returned if the file is not saved successfully.
        /// </returns>
        private async Task<string?> SaveFileAsync(IFormFile documentFile, Guid documentId)
        {
            string currentDirectory = string.Empty;

            try
            {
                currentDirectory = Directory.GetCurrentDirectory();
            }
            catch (Exception)
            {
                return null;
            }

            string basePath = Path.Combine(currentDirectory, "Documents");

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            string documentPath = Path.Combine(basePath, documentId.ToString());

            if (!Directory.Exists(documentPath))
            {
                Directory.CreateDirectory(documentPath);
            }

            string filePath = GenerateFileName(documentPath);

            if (string.IsNullOrEmpty(filePath))
            {
                return null;
            }

            try
            {
                using (Stream file = new FileStream(filePath, FileMode.CreateNew))
                {
                    await documentFile.CopyToAsync(file);
                }

                return filePath;
            }
            catch (Exception)
            {
                return null;
            }


        }

        /// <summary>
        ///     Generates a file name and checks that it is not in use.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>
        ///     A file path is returned that is not used.<br></br>
        ///     An empty string is returned if the method is unable to generate a name.
        /// </returns>
        private string GenerateFileName(string path)
        {
            int tryCount = 0;

            while (tryCount < 10)
            {
                Guid newFileName = Guid.NewGuid();

                string filePath = Path.Combine(path, (newFileName.ToString() + ".pdf"));

                if (!File.Exists(path))
                {
                    return filePath;
                }

                tryCount++;
            }

            return string.Empty;
        }

    }
}

﻿using Microsoft.AspNetCore.Http;
using SuperDoc.Customer.Repositories.Entities.Documents;
using SuperDoc.Customer.Services.Shared.StatusModels;

namespace SuperDoc.Customer.Services.Revisions
{
    public interface IRevisionService
    {
        Task<Revision?> GetRevisionByIdAsync(Guid revisionId);
        Task<IEnumerable<Revision>> GetRevisionsByDocumentIdWithDocumentSignaturesAndUsersAsync(Guid documentId);

        /// <summary>
        ///    This method will creae a new revision for the given document.<br></br>
        ///    And if the given email address don't exist in the database, a new user will be created.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="documentId"></param>
        /// <param name="emailAddresses"></param>
        /// <param name="documentFile"></param>
        /// <returns>
        ///     The created revision if the file was uploaded successfully.<br></br>
        ///     Revision is null if the file could not be uploaded. Read the error message for more information.
        /// </returns>
        Task<ResultModel<Revision>> SaveRevisionAsync(Guid userId, Guid documentId, string emailAddresses, IFormFile documentFile);

        /// <summary>
        ///    Sign a revision
        /// </summary>
        /// <param name="revisionId"></param>
        /// <param name="userId"></param>
        /// <param name="signature"></param>
        /// <param name="publicKey"></param>
        /// <returns>
        ///     DocumentSignature is returned if the revision is signed successfully.<br></br>
        ///     DocumentSignature is null if the revision is not signed successfully. Read the error message for more information.
        /// </returns>
        Task<ResultModel<DocumentSignature>> SignRevisionAsync(Guid revisionId, Guid userId, string signature, string publicKey);
    }
}
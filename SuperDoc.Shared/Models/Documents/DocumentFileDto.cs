using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SuperDoc.Shared.Models.Documents
{
    public class DocumentFileDto
    {
        [Required]
        public Guid DocumentId { get; set; }


        public IEnumerable<DocumentUserDto> SignatureUsers { get; set; } = new List<DocumentUserDto>();


        [Required]
        public IFormFile? DocumentFile { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SuperDoc.Shared.Models.Cases
{
    public class CreateOrUpdateCaseDto
    {
        public Guid? CaseId { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;


        [MaxLength(1024)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public Guid ResponsibleUserId { get; set; }

        public IEnumerable<Guid> CaseMangers { get; set; } = Array.Empty<Guid>();
    }
}

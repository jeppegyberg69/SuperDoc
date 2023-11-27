using System;
using System.Collections.Generic;

namespace SuperDoc.Shared.Models.Cases
{
    public class CaseDto
    {
        public Guid? CaseId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public CaseManagerDto ResponsibleUser { get; set; } = new CaseManagerDto();

        public IEnumerable<CaseManagerDto> CaseManagers { get; set; } = Array.Empty<CaseManagerDto>();
    }
}

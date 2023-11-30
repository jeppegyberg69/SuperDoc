namespace SuperDoc.Shared.Models.Cases
{
    public class CaseDto
    {
        public Guid? CaseId { get; set; }
        public int CaseNumber { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public CaseManagerDto ResponsibleUser { get; set; } = new CaseManagerDto();

        public IEnumerable<CaseManagerDto> CaseManagers { get; set; } = Array.Empty<CaseManagerDto>();
    }
}

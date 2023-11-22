using System;

namespace SuperDoc.Shared.Models.Cases
{
    public class CaseManagerDto
    {
        public Guid UserId { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}

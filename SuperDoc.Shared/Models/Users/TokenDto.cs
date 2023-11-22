using System;

namespace SuperDoc.Shared.Models.Users
{
    public class TokenDto
    {
        public string Token { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }
}

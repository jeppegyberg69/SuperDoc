using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SuperDoc.Shared.Models
{
    public class TokenDto
    {
        public string Token { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }
}

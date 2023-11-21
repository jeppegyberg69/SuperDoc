﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperDoc.Customer.Repositories.Entities.Users
{
    [Table("Users")]
    [Index(nameof(EmailAddress), IsUnique = true)]
    public class User
    {
        [Key]
        [Required]
        public Guid UserId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = string.Empty;

        [StringLength(128, MinimumLength = 1)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public Roles Role { get; set; }

        [Required]
        public bool IsUserSignedUp { get; set; }

        [Required]
        public bool IsDisabled { get; set; }

        public DateTime? LastLogin { get; set; }
        public DateTime? DateModified { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }
    }
}
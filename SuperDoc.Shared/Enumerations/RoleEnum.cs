using System.ComponentModel.DataAnnotations;

namespace SuperDoc.Shared.Enumerations;

public enum Role
{
    [Display(Name = "Ekstern Bruger")]
    User,

    [Display(Name = "Sagsbehandler")]
    CaseManager,

    [Display(Name = "Administrator")]
    Admin,

    [Display(Name = "Super Administrator")]
    SuperAdmin,
}

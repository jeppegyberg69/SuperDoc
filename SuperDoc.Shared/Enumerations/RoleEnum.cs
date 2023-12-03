using System.ComponentModel.DataAnnotations;

namespace SuperDoc.Shared.Enumerations;

/// <summary>
/// Different roles in the system, each associated with a specific level of access and responsibility.
/// </summary>
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

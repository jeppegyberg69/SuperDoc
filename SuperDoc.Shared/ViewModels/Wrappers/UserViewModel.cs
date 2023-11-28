using SuperDoc.Shared.Enumerations;
using SuperDoc.Shared.Models.Users;

namespace SuperDoc.Shared.ViewModels.Wrappers;

public class UserViewModel(TokenDto token) : BaseModelWrapper<TokenDto>(token)
{
    public string FirstName
    {
        get => Model.FirstName;
        set
        {
            Model.FirstName = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(FullName));
        }
    }

    public string LastName
    {
        get => Model.LastName;
        set
        {
            Model.LastName = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(FullName));
        }
    }

    public string FullName
    {
        get => $"{Model.FirstName} {Model.LastName}";
    }

    public string EmailAddress
    {
        get => Model.EmailAddress;
        set
        {
            Model.EmailAddress = value;
            OnPropertyChanged();
        }
    }

    public string? PhoneNumber
    {
        get => "-";
    }

    public Role Role
    {
        get => ConvertRole(Model.Role);
    }

    public string Token
    {
        get => Model.Token;
    }

    private static Role ConvertRole(string role)
    {
        switch (role)
        {
            case nameof(Role.CaseManager):
            {
                return Role.CaseManager;
            }

            case nameof(Role.Admin):
            {
                return Role.Admin;
            }

            case nameof(Role.SuperAdmin):
            {
                return Role.SuperAdmin;
            }

            default:
            {
                return Role.User;
            }
        }
    }
}

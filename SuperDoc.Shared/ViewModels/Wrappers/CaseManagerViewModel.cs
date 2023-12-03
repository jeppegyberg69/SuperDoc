using SuperDoc.Shared.Models.Cases;

namespace SuperDoc.Shared.ViewModels.Wrappers;

public class CaseManagerViewModel(CaseManagerDto model) : BaseModelWrapper<CaseManagerDto>(model)
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
}

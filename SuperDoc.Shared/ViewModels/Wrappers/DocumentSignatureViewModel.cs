using SuperDoc.Shared.Models.Documents;

namespace SuperDoc.Shared.ViewModels.Wrappers;

public class DocumentSignatureViewModel(DocumentSignatureDto model) : BaseModelWrapper<DocumentSignatureDto>(model)
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

    public string PublicKey
    {
        get => Model.PublicKey;
        set
        {
            Model.PublicKey = value;
            OnPropertyChanged();
        }
    }

    public string Signature
    {
        get => Model.Signature;
        set
        {
            Model.Signature = value;
            OnPropertyChanged();
        }
    }

    public DateTime? DateSigned
    {
        get => Model.DateSigned?.ToLocalTime();
    }
}

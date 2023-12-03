using System.Collections.ObjectModel;

using SuperDoc.Shared.Models.Revisions;

namespace SuperDoc.Shared.ViewModels.Wrappers;

public class RevisionViewModel : BaseModelWrapper<RevisionDto>
{
    public RevisionViewModel(RevisionDto model) : base(model)
    {
        Signatures = new ObservableCollection<DocumentSignatureViewModel>(model.Signatures.Select(signature => new DocumentSignatureViewModel(signature)));
    }

    public Guid RevisionId
    {
        get => Model.RevisionId;
    }

    private int _revisionNumber;
    public int RevisionNumber
    {
        get => _revisionNumber;
        set
        {
            if (SetProperty(ref _revisionNumber, value))
            {
                OnPropertyChanged(nameof(RevisionName));
            }
        }
    }

    public string RevisionName
    {
        get => $"Revision #{RevisionNumber}";
    }

    private ObservableCollection<DocumentSignatureViewModel> _signatures = null!;
    public ObservableCollection<DocumentSignatureViewModel> Signatures
    {
        get => _signatures;
        init => SetProperty(ref _signatures, value);
    }

    public DateTime DateUploaded
    {
        get => Model.DateUploaded.ToLocalTime();
    }
}

using System.Collections.ObjectModel;

using SuperDoc.Shared.Models.Documents;

namespace SuperDoc.Shared.ViewModels.Wrappers;

public class DocumentViewModel : BaseModelWrapper<DocumentDto>
{
    public DocumentViewModel(DocumentDto model) : base(model)
    {
        ExternalUsers = new ObservableCollection<CaseManagerViewModel>(model.ExternalUsers.Select(user => new CaseManagerViewModel(user)));
    }

    public Guid DocumentId
    {
        get => Model.DocumentId;
    }

    public Guid CaseId
    {
        get => Model.CaseId;
    }

    public string Title
    {
        get => Model.Title;
        set
        {
            Model.Title = value;
            OnPropertyChanged();
        }
    }

    public string Description
    {
        get => Model.Description;
        set
        {
            Model.Description = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<CaseManagerViewModel> _externalUsers = null!;
    public ObservableCollection<CaseManagerViewModel> ExternalUsers
    {
        get => _externalUsers;
        init => SetProperty(ref _externalUsers, value);
    }

    public DateTime DateModified
    {
        get => Model.DateModified.ToLocalTime();
    }

    public DateTime DateCreated
    {
        get => Model.DateCreated.ToLocalTime();
    }
}

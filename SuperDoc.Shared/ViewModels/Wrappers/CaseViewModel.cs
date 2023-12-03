using System.Collections.ObjectModel;

using SuperDoc.Shared.Models.Cases;

namespace SuperDoc.Shared.ViewModels.Wrappers;

public class CaseViewModel : BaseModelWrapper<CaseDto>
{
    public CaseViewModel(CaseDto model) : base(model)
    {
        ResponsibleCaseManager = new CaseManagerViewModel(model.ResponsibleUser);
        CaseManagers = new ObservableCollection<CaseManagerViewModel>(model.CaseManagers.Select(caseManager => new CaseManagerViewModel(caseManager)));
    }

    public Guid CaseId
    {
        get => Model.CaseId ?? Guid.Empty;
    }

    public int CaseNumber
    {
        get => Model.CaseNumber;
        set
        {
            Model.CaseNumber = value;
            OnPropertyChanged();
        }
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

    private CaseManagerViewModel? _responsibleCaseManager;
    public CaseManagerViewModel? ResponsibleCaseManager
    {
        get => _responsibleCaseManager;
        init => SetProperty(ref _responsibleCaseManager, value);
    }

    private ObservableCollection<CaseManagerViewModel> _caseManagers = null!;
    public ObservableCollection<CaseManagerViewModel> CaseManagers
    {
        get => _caseManagers;
        init => SetProperty(ref _caseManagers, value);
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

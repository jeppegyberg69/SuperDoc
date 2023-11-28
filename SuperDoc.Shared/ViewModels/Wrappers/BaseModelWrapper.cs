using CommunityToolkit.Mvvm.ComponentModel;

namespace SuperDoc.Shared.ViewModels.Wrappers;

public abstract class BaseModelWrapper<TModel> : ObservableObject
{
    public BaseModelWrapper(TModel model)
    {
        Model = model;
    }

    private TModel _model = default!;
    protected TModel Model
    {
        get => _model;
        private set => SetProperty(ref _model, value);
    }

    public TModel GetModel()
    {
        return Model;
    }
}

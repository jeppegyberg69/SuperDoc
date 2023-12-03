using CommunityToolkit.Mvvm.ComponentModel;

namespace SuperDoc.Shared.ViewModels.Wrappers;

/// <summary>
/// Provides a base abstract class for wrapper classes that encapsulate and extend functionality for a specific model type.
/// </summary>
/// <typeparam name="TModel">The type of the underlying model being wrapped.</typeparam>
public abstract class BaseModelWrapper<TModel> : ObservableObject
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BaseModelWrapper{TModel}"/> class with the specified underlying model.
    /// </summary>
    /// <param name="model">The model to be wrapped by the instance.</param>
    public BaseModelWrapper(TModel model)
    {
        Model = model;
    }

    private TModel _model = default!;
    /// <summary>
    /// Gets the underlying model associated with the wrapper view model.
    /// </summary>
    protected TModel Model
    {
        get => _model;
        init => SetProperty(ref _model, value);
    }

    /// <summary>
    /// Gets the underlying model associated with the wrapper view model.
    /// </summary>
    /// <returns>The model instance associated with the wrapper view model.</returns>
    public TModel GetModel()
    {
        return Model;
    }
}

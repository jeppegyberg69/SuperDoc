namespace SuperDoc.Customer.Services.Shared.StatusModels
{
    public class ResultModel<T>
    {

        public T? Result { get; set; }
        public string? ErrorMessage { get; set; }

        public ResultModel(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public ResultModel(T result)
        {
            Result = result;
        }
    }
}

using SuperDoc.Customer.Repositories.Entities.Cases;

namespace SuperDoc.Customer.Services.Cases.StatusModels
{
    public class CreateOrUpdateCaseStatusModel
    {
        public CreateOrUpdateCaseStatusModel(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public CreateOrUpdateCaseStatusModel(Case docCase)
        {
            Case = docCase;
        }

        public Case? Case { get; set; }
        public string? ErrorMessage { get; set; }
    }
}

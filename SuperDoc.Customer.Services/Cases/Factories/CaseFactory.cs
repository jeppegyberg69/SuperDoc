using SuperDoc.Customer.Repositories.Entities.Users;
using SuperDoc.Shared.Models.Cases;

namespace SuperDoc.Customer.Services.Cases.Factories
{
    public class CaseFactory : ICaseFactory
    {
        public IEnumerable<CaseManagerDto> ConverUsersToCaseManagerDtos(IEnumerable<User> users)
        {
            var managers = new List<CaseManagerDto>();

            if (users != null)
            {
                foreach (var user in users)
                {

                    managers.Add(new CaseManagerDto
                    {
                        UserId = user.UserId,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        EmailAddress = user.EmailAddress
                    });
                }
            }

            return managers;
        }
    }
}

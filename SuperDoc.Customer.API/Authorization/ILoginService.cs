﻿using SuperDoc.Customer.Repositories.Entities.Users;
using SuperDoc.Shared.Models.Users;

namespace SuperDoc.Customer.API.Authorization
{
    public interface ILoginService
    {
        TokenDto GenerateToken(User user, DateTime validFrom, DateTime validTo);
    }
}
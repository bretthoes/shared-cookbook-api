using SharedCookbookApi.Controllers;
using SharedCookbookApi.Data.Entities;
using SharedCookbookApi.Extensions;
using System;

namespace SharedCookbookApi.Services
{
    public class AuthService : IAuthService
    {
        public string HashPassword(string password)
        {
            return PasswordHasherExtension.Hash(password);
        }

        public bool ValidatePassword(string password, string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(password) 
                || string.IsNullOrWhiteSpace(hashedPassword))
            {
                return false;
            }

            return PasswordHasherExtension.Verify(password, hashedPassword);
        }
    }
}

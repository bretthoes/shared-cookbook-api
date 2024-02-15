using SharedCookbookApi.Extensions;

namespace SharedCookbookApi.Services
{
    public class AuthService : IAuthService
    {
        public string HashPassword(string password)
        {
            return PasswordHasherExtension.Hash(password);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hashedPassword))
            {
                return false;
            }

            return PasswordHasherExtension.Verify(password, hashedPassword);
        }
    }
}

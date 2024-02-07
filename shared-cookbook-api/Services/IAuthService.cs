namespace SharedCookbookApi.Services
{
    public interface IAuthService
    {
        public string HashPassword(string password);
        bool ValidatePassword(string password, string hashedPassword);
    }
}

namespace SharedCookbookApi.Services;

public interface IAuthService
{
    public string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}

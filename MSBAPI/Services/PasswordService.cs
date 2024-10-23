using Microsoft.AspNetCore.Identity;

public class PasswordService
{
    private readonly IPasswordHasher<User> _passwordHasher;

    public PasswordService(IPasswordHasher<User> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public string HashPassword(User user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    public bool VerifyPassword(User user, string password, string hashedPassword)
    {
        return _passwordHasher.VerifyHashedPassword(user, hashedPassword, password) != PasswordVerificationResult.Failed;
    }
}
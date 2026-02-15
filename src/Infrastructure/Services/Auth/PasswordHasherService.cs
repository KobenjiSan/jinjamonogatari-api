using Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services.Auth;

// Implements password hashing using ASP.NET Identity's built-in hasher.
// This provides salted, secure hashes without storing plaintext passwords.
public class PasswordHasherService : IPasswordHasher
{
    // Handles salting + secure algorithms internally
    private readonly PasswordHasher<object> _hasher = new();

    // Hash a plaintext password before storing in DB.
    public string Hash(string password)
        => _hasher.HashPassword(new object(), password);

    // Verify a login attempt by comparing provided password
    // against stored hash.
    public bool Verify(string hash, string providedPassword)
        => _hasher.VerifyHashedPassword(new object(), hash, providedPassword)
           == PasswordVerificationResult.Success;
}

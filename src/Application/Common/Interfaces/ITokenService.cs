namespace Application.Common.Interfaces;

public interface ITokenService
{
    // Access token for a known user
    string CreateAccessToken(int userId, string email, string role);

    // Refresh token (raw string) to send to client once
    string CreateRefreshToken();

    // Hash for DB storage/lookup
    string HashRefreshToken(string rawToken);
}
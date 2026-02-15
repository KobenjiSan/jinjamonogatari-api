namespace Application.Common.Interfaces;

public interface ITokenService
{
    // Access token for a known user
    string CreateAccessToken(int userId, string email);
}

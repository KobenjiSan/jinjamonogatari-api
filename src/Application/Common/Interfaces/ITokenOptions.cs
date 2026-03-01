namespace Application.Common.Interfaces;

public interface ITokenOptions
{
    int AccessTokenMinutes { get; }
    int RefreshTokenDays { get; }
}
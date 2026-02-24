using System.Security.Claims;

namespace API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var sub =
            user.FindFirstValue(ClaimTypes.NameIdentifier) ??
            user.FindFirstValue("sub");

        if (string.IsNullOrWhiteSpace(sub) || !int.TryParse(sub, out var userId))
            throw new UnauthorizedAccessException("Invalid token.");

        return userId;
    }
}
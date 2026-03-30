using Application.Common.Exceptions;

namespace Application.Common.Policies;

public static class ShrineWritePolicy
{
    public static void EnsureCanModify(string shrineStatus, string userRole)
    {
        if (shrineStatus == "published")
            throw new ForbiddenException("Published shrines cannot be edited.");

        if (shrineStatus == "review" && userRole == "Editor")
            throw new ForbiddenException("Editors cannot edit shrines under review.");
    }
}
using MediatR;

namespace Application.Features.Users.Commands.UpdateMyProfile;

public record UpdateMyProfileCommand(
    int UserId,
    bool HasFirstName,
    string? FirstName,
    bool HasLastName,
    string? LastName,
    bool HasPhone,
    string? Phone
) : IRequest<UpdateMyProfileResult>;

public record UpdateMyProfileResult(
    int UserId,
    string Email,
    string Username,
    string? FirstName,
    string? LastName,
    string? Phone
);
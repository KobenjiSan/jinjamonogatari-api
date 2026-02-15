using MediatR;

namespace Application.Features.Users.Queries.GetMe;

public record GetMeQuery(int UserId) : IRequest<GetMeResult>;

public record GetMeResult(
    int UserId,
    string Email,
    string Username,
    string? Phone,
    string? FirstName,
    string? LastName
);

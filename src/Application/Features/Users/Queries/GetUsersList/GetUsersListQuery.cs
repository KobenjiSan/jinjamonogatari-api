using Application.Features.Users.Models;
using Domain.Enums;
using MediatR;

namespace Application.Features.Users.Queries.GetUsersList;

// QUERY
public record GetUsersListQuery(
    string? Role,
    string? SearchQuery,
    UserSort? Sort,
    int Page = 1,
    int PageSize = 5
) : IRequest<GetUsersListResult>;

// RESULT
public record GetUsersListResult(IReadOnlyList<UserListDto> Users, int TotalCount);
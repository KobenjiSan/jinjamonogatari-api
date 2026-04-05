using Application.Common.Exceptions;
using Application.Features.Users.Models;
using Application.Features.Users.Services;
using MediatR;

namespace Application.Features.Users.Queries.GetUsersList;

public class GetUsersListHandler : IRequestHandler<GetUsersListQuery, GetUsersListResult>
{
    private readonly IUserReadService _readService;

    public GetUsersListHandler(IUserReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetUsersListResult> Handle(GetUsersListQuery request, CancellationToken ct)
    {
        var (users, total) = await _readService.GetUsersListAsync(request, ct);
        return new GetUsersListResult(users, total);
    }
}
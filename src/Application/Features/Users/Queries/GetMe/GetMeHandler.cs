using Application.Common.Exceptions;
using Application.Features.Users.Services;
using MediatR;

namespace Application.Features.Users.Queries.GetMe;

public class GetMeHandler : IRequestHandler<GetMeQuery, GetMeResult>
{
    private readonly IUserReadService _readService;

    public GetMeHandler(IUserReadService readService)
    {
        _readService = readService;
    }

    public async Task<GetMeResult> Handle(GetMeQuery request, CancellationToken ct)
    {
        var user = await _readService.FindByIdAsync(request.UserId, ct);

        if (user is null)
            throw new NotFoundException("User not found.");

        return new GetMeResult(
            user.UserId,
            user.Email,
            user.Username,
            user.Phone,
            user.FirstName,
            user.LastName
        );
    }
}

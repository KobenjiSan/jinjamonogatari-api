using Application.Common.Exceptions;
using Application.Features.Shrines.Services;
using Application.Features.Users.Services;
using MediatR;

namespace Application.Features.Users.Commands.UpdateUserRole;

public class UpdateUserRoleHandler : IRequestHandler<UpdateUserRoleCommand>
{
    private readonly IUserWriteService _writeService;

    public UpdateUserRoleHandler(IUserWriteService writeService)
    {
        _writeService = writeService;
    }

    public async Task<Unit> Handle(UpdateUserRoleCommand request, CancellationToken ct)
    {
        await _writeService.UpdateUserRoleAsync(request.UserId, request.UserRole, ct);
        return Unit.Value;
    }
}
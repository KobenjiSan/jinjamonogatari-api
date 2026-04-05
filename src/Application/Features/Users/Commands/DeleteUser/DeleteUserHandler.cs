using Application.Common.Exceptions;
using Application.Features.Shrines.Services;
using Application.Features.Users.Services;
using MediatR;

namespace Application.Features.Users.Commands.DeleteUser;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserWriteService _writeService;

    public DeleteUserHandler(IUserWriteService writeService)
    {
        _writeService = writeService;
    }

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken ct)
    {
        await _writeService.DeleteUserAsync(request.UserId, ct);
        return Unit.Value;
    }
}
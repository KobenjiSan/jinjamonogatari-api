using MediatR;

namespace Application.Features.Shrines.Commands.UpdateShrineNotes;

// COMMAND
public record UpdateShrineNotesCommand(
    int UserId,
    string Username,
    string UserRole,
    int ShrineId,
    string Notes
) : IRequest<Unit>;
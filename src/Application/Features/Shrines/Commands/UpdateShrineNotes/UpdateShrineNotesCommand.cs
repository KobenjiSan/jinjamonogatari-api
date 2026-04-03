using MediatR;

namespace Application.Features.Shrines.Commands.UpdateShrineNotes;

// COMMAND
public record UpdateShrineNotesCommand(
    string UserRole,
    int ShrineId,
    string Notes
) : IRequest<Unit>;
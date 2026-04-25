using Application.Features.Etiquette.Services;
using MediatR;

namespace Application.Features.Etiquette.Commands.DeleteGlance;

public class DeleteGlanceHandler : IRequestHandler<DeleteGlanceCommand, Unit>
{
    private readonly IEtiquetteWriteService _writeService;

    public DeleteGlanceHandler(IEtiquetteWriteService writeService)
    {
        _writeService = writeService;
    }

    public async Task<Unit> Handle(DeleteGlanceCommand request, CancellationToken ct)
    {
        await _writeService.DeleteGlanceAsync(request.TopicId, ct);
        return Unit.Value;
    }
}
using Application.Features.Etiquette.Services;
using MediatR;

namespace Application.Features.Etiquette.Commands.UpdateGlance;

public class UpdateGlanceHandler : IRequestHandler<UpdateGlanceCommand, Unit>
{
    private readonly IEtiquetteWriteService _writeService;

    public UpdateGlanceHandler(IEtiquetteWriteService writeService)
    {
        _writeService = writeService;
    }

    public async Task<Unit> Handle(UpdateGlanceCommand request, CancellationToken ct)
    {
        await _writeService.UpdateGlanceAsync(request.TopicId, request.Request, ct);
        return Unit.Value;
    }
}
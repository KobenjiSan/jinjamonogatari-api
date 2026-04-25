using Application.Features.Etiquette.Services;
using MediatR;

namespace Application.Features.Etiquette.Commands.UpdateEtiquette;

public class UpdateEtiquetteHandler : IRequestHandler<UpdateEtiquetteCommand, Unit>
{
    private readonly IEtiquetteWriteService _writeService;

    public UpdateEtiquetteHandler(IEtiquetteWriteService writeService)
    {
        _writeService = writeService;
    }

    public async Task<Unit> Handle(UpdateEtiquetteCommand request, CancellationToken ct)
    {
        await _writeService.UpdateEtiquetteAsync(request.TopicId, request.Request, ct);
        return Unit.Value;
    }
}
using Application.Features.Etiquette.Services;
using MediatR;

namespace Application.Features.Etiquette.Commands.DeleteEtiquette;

public class DeleteEtiquetteHandler : IRequestHandler<DeleteEtiquetteCommand, Unit>
{
    private readonly IEtiquetteWriteService _writeService;

    public DeleteEtiquetteHandler(IEtiquetteWriteService writeService)
    {
        _writeService = writeService;
    }

    public async Task<Unit> Handle(DeleteEtiquetteCommand request, CancellationToken ct)
    {
        await _writeService.DeleteEtiquetteAsync(request.TopicId, ct);
        return Unit.Value;
    }
}
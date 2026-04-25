using Application.Features.Etiquette.Services;
using MediatR;

namespace Application.Features.Etiquette.Commands.CreateEtiquette;

public class CreateEtiquetteHandler : IRequestHandler<CreateEtiquetteCommand, Unit>
{
    private readonly IEtiquetteWriteService _writeService;

    public CreateEtiquetteHandler(IEtiquetteWriteService writeService)
    {
        _writeService = writeService;
    }

    public async Task<Unit> Handle(CreateEtiquetteCommand request, CancellationToken ct)
    {
        await _writeService.CreateEtiquetteAsync(request.Request, ct);
        return Unit.Value;
    }
}
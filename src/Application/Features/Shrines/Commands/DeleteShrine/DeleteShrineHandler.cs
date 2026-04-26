using Application.Features.Audits.Services;
using Application.Features.Shrines.Services;
using MediatR;

namespace Application.Features.Shrines.Commands.DeleteShrine;

public class DeleteShrineHandler : IRequestHandler<DeleteShrineCommand, Unit>
{
    private readonly IShrineWriteService _shrineWriteService;
    private readonly IAuditService _audit;

    public DeleteShrineHandler(
        IShrineWriteService shrineWriteService,
        IAuditService audit
    )
    {
        _shrineWriteService = shrineWriteService;
        _audit = audit;
    }

    public async Task<Unit> Handle(DeleteShrineCommand request, CancellationToken ct)
    {
        try
        {
            // TODO: need to delete all images under this shrines id in cloudinary

            await _shrineWriteService.DeleteShrineAsync(request.ShrineId, ct);

            await _audit.LogAsync(request.UserId, request.Username, "DeletedShrine", $"Shrine Management (Shrine #{request.ShrineId})", true, null, ct);
        }
        catch (Exception e)
        {
            try
            {
                await _audit.LogAsync(request.UserId, request.Username, "DeletedShrine", $"Shrine Management (Shrine #{request.ShrineId})", false, e.Message, ct);
            }
            catch { }
            throw;
        }

        return Unit.Value;
    }
}
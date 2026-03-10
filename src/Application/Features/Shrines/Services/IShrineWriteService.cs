namespace Application.Features.Shrines.Services;

public interface IShrineWriteService
{
    Task UpdateShrineMetaAsync(int shrineId, UpdateShrineMetaRequest request, CancellationToken ct);
}
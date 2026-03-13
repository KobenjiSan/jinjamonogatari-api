namespace Application.Features.Shrines.Services;

public interface IShrineWriteService
{
    Task UpdateShrineMetaAsync(int shrineId, UpdateShrineMetaRequest request, CancellationToken ct);
    
    Task CreateKamiInShrineAsync(
        int shrineId,
        CreateKamiInShrineRequest request,
        CancellationToken ct
    );

    Task LinkKamiToShrineAsync(int shrineId, int kamiId, CancellationToken ct);

    Task UnlinkKamiToShrineAsync(int shrineId, int kamiId, CancellationToken ct);

    Task UpdateKamiAsync(int kamiId, UpdateKamiRequest request, CancellationToken ct);
}
namespace Application.Features.Shrines.Services;

public interface IShrineWriteService
{
    // META
    Task UpdateShrineMetaAsync(int shrineId, UpdateShrineMetaRequest request, CancellationToken ct);
    
    // KAMI
    Task CreateKamiInShrineAsync(
        int shrineId,
        CreateKamiInShrineRequest request,
        CancellationToken ct
    );
    Task LinkKamiToShrineAsync(int shrineId, int kamiId, CancellationToken ct);
    Task UnlinkKamiToShrineAsync(int shrineId, int kamiId, CancellationToken ct);
    Task UpdateKamiAsync(int kamiId, UpdateKamiRequest request, CancellationToken ct);

    // HISTORY
    Task CreateHistoryAsync(
        int shrineId,
        CreateHistoryRequest request,
        CancellationToken ct
    );
    Task DeleteHistoryAsync(int historyId, CancellationToken ct);
    Task UpdateHistoryAsync(int historyId, UpdateHistoryRequest request, CancellationToken ct);

    // FOLKLORE
    Task CreateFolkloreAsync(
        int shrineId,
        CreateFolkloreRequest request,
        CancellationToken ct
    );
    Task DeleteFolkloreAsync(int folkloreId, CancellationToken ct);
    Task UpdateFolkloreAsync(int folkloreId, UpdateFolkloreRequest request, CancellationToken ct);
}
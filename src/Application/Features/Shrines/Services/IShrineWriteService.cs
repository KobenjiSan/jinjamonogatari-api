using Application.Common.Models.Images;

namespace Application.Features.Shrines.Services;

public interface IShrineWriteService
{
    // META
    Task UpdateShrineMetaAsync(int shrineId, UpdateShrineMetaRequest request, CancellationToken ct);

    // HERO IMAGE
    Task<ImageFullDto> CreateHeroImageAsync(
        int shrineId,
        CreateImageFormRequest request,
        string publicId,
        CancellationToken ct
    );

    Task<ImageFullDto> UpdateHeroImageAsync(
        int shrineId,
        UpdateImageFormRequest request,
        CancellationToken ct
    );

    Task DeleteHeroImageAsync(
        int shrineId,
        int imageId,
        CancellationToken ct
    );

    // NOTES
    Task UpdateShrineNotesAsync(int shrineId, string notes, CancellationToken ct);

    // KAMI
    Task CreateKamiInShrineAsync(
        int shrineId,
        CreateKamiInShrineRequest request,
        string? publicId,
        CancellationToken ct
    );
    Task LinkKamiToShrineAsync(int shrineId, int kamiId, CancellationToken ct);
    Task UnlinkKamiToShrineAsync(int shrineId, int kamiId, CancellationToken ct);

    // HISTORY
    Task CreateHistoryAsync(
        int shrineId,
        CreateHistoryRequest request,
        string? publicId,
        CancellationToken ct
    );
    Task DeleteHistoryAsync(int historyId, CancellationToken ct);
    Task UpdateHistoryAsync(int historyId, UpdateHistoryRequest request, string? publicId, CancellationToken ct);

    // FOLKLORE
    Task CreateFolkloreAsync(
        int shrineId,
        CreateFolkloreRequest request,
        string? publicId,
        CancellationToken ct
    );
    Task DeleteFolkloreAsync(int folkloreId, CancellationToken ct);
    Task UpdateFolkloreAsync(int folkloreId, UpdateFolkloreRequest request, string? publicId, CancellationToken ct);

    // GALLERY
    Task CreateGalleryImageAsync(
        int shrineId,
        CreateImageFormRequest request,
        string publicId,
        CancellationToken ct
    );
    Task DeleteGalleryImageAsync(int imageId, CancellationToken ct);
    Task UpdateGalleryImageAsync(int imageId, UpdateImageFormRequest request, CancellationToken ct);

    Task ImportShrinesAsync(ImportShrinesRequest request, CancellationToken ct);

    Task CreateShrineAsync(CreateShrineRequest request, CancellationToken ct);
    Task DeleteShrineAsync(int shrineId, CancellationToken ct);

    Task UpdateShrineStatus(int shrineId, string status, CancellationToken ct);

    Task SubmitShrineForReview(int shrineId, int userId, CancellationToken ct);
    Task RejectShrineForReview(int shrineId, int userId, string message, CancellationToken ct);
    Task PublishShrineForReview(int shrineId, int userId, CancellationToken ct);
}
using Application.Common.Models.Citations;
using Application.Common.Models.Images;
using Application.Features.Shrines.Models;
using Application.Features.Shrines.Queries.GetShrineListCMS;

namespace Application.Features.Shrines.Services;

public interface IShrineReadService
{
    Task<IReadOnlyList<ShrineMapPointDto>> GetShrineMapPointsAsync(CancellationToken ct);

    Task<ShrinePreviewDto?> GetShrinePreviewAsync(string slug, double? lat, double? lon, CancellationToken ct);

    Task<IReadOnlyList<ShrineCardDto>> GetShrineListViewAsync(
        double? lat,
        double? lon,
        string? q,
        CancellationToken ct
    );

    Task<ShrineMetaDto?> GetShrineMetaBySlugAsync(
        string slug,
        double? lat,
        double? lon,
        CancellationToken ct
    );

    Task<IReadOnlyList<KamiReadDto>> GetShrineKamiBySlugAsync(string slug, CancellationToken ct);

    Task<IReadOnlyList<HistoryReadDto>> GetShrineHistoryBySlugAsync(string slug, CancellationToken ct);

    Task<IReadOnlyList<FolkloreReadDto>> GetShrineFolkloreBySlugAsync(string slug, CancellationToken ct);

    Task<IReadOnlyList<GalleryListItemDto>> GetShrineGalleryBySlugAsync(string slug, CancellationToken ct);

    Task<ImageFullDto?> GetImageByIdAsync(int id, CancellationToken ct);

    Task<bool> DoesShrineExistAsync(int shrineId, CancellationToken ct);

    // CMS
    Task<(IReadOnlyList<ShrineListCMSDto>, int)> GetShrineListCMSAsync(GetShrineListCMSQuery request, CancellationToken ct);
    Task<ShrineMetaCMSDto?> GetShrineMetaByIdCMSAsync(int id, CancellationToken ct);
    Task<string?> GetShrineNotesByIdCMSAsync(int id, CancellationToken ct);
    Task<IReadOnlyList<KamiReadCMSDto>> GetShrineKamiByIdCMSAsync(int id, CancellationToken ct);

    Task<IReadOnlyList<KamiReadCMSDto>> GetAllKamiListCMSAsync(CancellationToken ct);

    Task<IReadOnlyList<HistoryReadCMSDto>> GetShrineHistoryByIdCMSAsync(int id, CancellationToken ct);
    
    Task<IReadOnlyList<FolkloreReadCMSDto>> GetShrineFolkloreByIdCMSAsync(int id, CancellationToken ct);
    
    Task<IReadOnlyList<ImageCMSDto>> GetShrineGalleryByIdCMSAsync(int id, CancellationToken ct);

    Task<ShrineAuditSnapshot?> GetShrineAuditSnapshotAsync(int shrineId, CancellationToken ct);
    Task<List<ShrineCitationCMSDto>> GetShrineCitationsByIdCMSAsync(int shrineId, CancellationToken ct);

    Task<List<CitationCMSDto>> GetShrineCitationsDropdownByIdCMSAsync(int shrineId, CancellationToken ct);

    Task<string> GetShrineStatusByIdCMSAsync(int id, CancellationToken ct);
    Task<int> GetShrineIdByHistoryIdCMSAsync(int HistoryId, CancellationToken ct);
    Task<int> GetShrineIdByFolkloreIdCMSAsync(int FolkloreId, CancellationToken ct);
    Task<int> GetShrineIdByImageIdCMSAsync(int ImageId, CancellationToken ct);

    Task<List<string>> GetExistingImportIdsAsync(List<string> importIds, CancellationToken ct);
    
    Task<IReadOnlyList<ShrineReviewDto>> GetShrineReviewHistoryAsync(int shrineId, CancellationToken ct);
    Task<bool> IsShrineRecentlyRejectedAsync(int shrineId, CancellationToken ct);
    Task<string?> GetImagePublicIdCMSAsync(int imageId, CancellationToken ct);
    Task<string?> GetHistoryImagePublicIdCMSAsync(int historyId, CancellationToken ct);
    Task<string?> GetFolkloreImagePublicIdCMSAsync(int folkloreId, CancellationToken ct);
}
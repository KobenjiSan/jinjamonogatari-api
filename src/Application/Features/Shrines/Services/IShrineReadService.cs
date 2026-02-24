using Application.Common.Models.Images;
using Application.Features.Shrines.Models;

namespace Application.Features.Shrines.Services;

public interface IShrineReadService
{
    Task<IReadOnlyList<ShrineMapPointDto>> GetShrineMapPointsAsync(CancellationToken ct);

    Task<ShrinePreviewDto?> GetShrinePreviewAsync(string slug, CancellationToken ct);

    Task<IReadOnlyList<ShrineCardDto>> GetShrineListViewAsync(CancellationToken ct);

    Task<ShrineMetaDto?> GetShrineMetaBySlugAsync(string slug, CancellationToken ct);

    Task<IReadOnlyList<KamiReadDto>> GetShrineKamiBySlugAsync(string slug, CancellationToken ct);

    Task<IReadOnlyList<HistoryReadDto>> GetShrineHistoryBySlugAsync(string slug, CancellationToken ct);

    Task<IReadOnlyList<FolkloreReadDto>> GetShrineFolkloreBySlugAsync(string slug, CancellationToken ct);

    Task<IReadOnlyList<GalleryListItemDto>> GetShrineGalleryBySlugAsync(string slug, CancellationToken ct);

    Task<ImageFullDto?> GetImageByIdAsync(int id, CancellationToken ct);

    Task<bool> DoesShrineExistAsync(int shrineId, CancellationToken ct);
}
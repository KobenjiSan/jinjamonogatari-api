using Application.Features.Kami.Queries.GetAllKamiCMS;
using Application.Features.Shrines.Models;

namespace Application.Features.Kami.Services;

public interface IKamiService
{
    Task<(IReadOnlyList<KamiReadCMSDto>, int)> GetAllKamiCMSAsync(GetAllKamiCMSQuery request, CancellationToken ct);

    Task CreateKamiAsync(CreateKamiInShrineRequest request, string? publicId, CancellationToken ct);
    Task DeleteKamiAsync(int kamiId, CancellationToken ct);
    Task UpdateKamiAsync(int kamiId, UpdateKamiRequest request, string? publicId, CancellationToken ct);

    Task<string?> GetKamiImagePublicIdCMSAsync(int kamiId, CancellationToken ct);
}
using Application.Features.Kami.Models;
using Application.Features.Kami.Queries.GetAllKamiCMS;
using Application.Features.Shrines.Models;

namespace Application.Features.Kami.Services;

public interface IKamiService
{
    Task<(IReadOnlyList<KamiReadCMSDto>, int)> GetAllKamiCMSAsync(GetAllKamiCMSQuery request, CancellationToken ct);

    Task<int> CreateKamiAsync(CreateKamiInShrineRequest request, string? publicId, CancellationToken ct);
    Task DeleteKamiAsync(int kamiId, CancellationToken ct);
    Task UpdateKamiAsync(int kamiId, UpdateKamiRequest request, string? publicId, CancellationToken ct);

    Task<string?> GetKamiImagePublicIdCMSAsync(int kamiId, CancellationToken ct);

    Task<KamiReadCMSDto?> GetKamiByIdAsync(int kamiId, CancellationToken ct);

    Task SubmitKamiForReviewAsync(int kamiId, int userId, CancellationToken ct);
    Task RejectKamiForReviewAsync(int kamiId, int userId, string message, CancellationToken ct);
    Task PublishKamiForReviewAsync(int kamiId, int userId, CancellationToken ct);
    Task<IReadOnlyList<KamiReviewDto>> GetKamiReviewHistoryAsync(int kamiId, CancellationToken ct);
}
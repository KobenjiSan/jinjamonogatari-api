using Application.Features.Shrines.Models;
using Application.Features.Tags.Models;
using Application.Features.Tags.Queries.GetAllTagsListCMS;

namespace Application.Features.Tags.Services;

public interface ITagsService
{
    Task<(IReadOnlyList<TagCMSDto>, int)> GetAllTagsListCMSAsync(GetAllTagsListCMSQuery request, CancellationToken ct);
    Task<IReadOnlyList<TagDto>> GetAllTagsDropdownAsync(CancellationToken ct);

    Task CreateTagAsync(TagRequest request, CancellationToken ct);
    Task DeleteTagAsync(int tagId, CancellationToken ct);
    Task UpdateTagAsync(int tagId, TagRequest request, CancellationToken ct);
}
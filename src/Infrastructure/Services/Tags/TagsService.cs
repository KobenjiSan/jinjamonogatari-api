using Application.Common.Exceptions;
using Application.Features.Shrines.Models;
using Application.Features.Tags.Models;
using Application.Features.Tags.Queries.GetAllTagsListCMS;
using Application.Features.Tags.Services;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Tags;

public class TagsService : ITagsService
{
    private readonly AppDbContext _db;

    public TagsService(AppDbContext db)
    {
        _db = db;
    }

    #region Tags List

    public async Task<(IReadOnlyList<TagCMSDto>, int)> GetAllTagsListCMSAsync(GetAllTagsListCMSQuery request, CancellationToken ct)
    {
        var query = _db.Tags.AsNoTracking();

        // Search Query
        if(!string.IsNullOrWhiteSpace(request.SearchQuery))
        {
            var search = request.SearchQuery.Trim().ToLower();

            query = query.Where(t =>
                (t.TitleEn != null && t.TitleEn.ToLower().Contains(search)) ||
                (t.TitleJp != null && t.TitleJp.ToLower().Contains(search))
            );
        }

        // Sort Type / Direction
        var sort = request.Sort ?? TagsSort.IdAsc;
        query = sort switch
        {
            TagsSort.TitleAsc => query.OrderBy(t => t.TitleEn),
            TagsSort.TitleDesc => query.OrderByDescending(t => t.TitleEn),
            TagsSort.UpdatedAsc => query.OrderBy(t => t.UpdatedAt),
            TagsSort.UpdatedDesc => query.OrderByDescending(t => t.UpdatedAt),
            TagsSort.IdAsc => query.OrderBy(t => t.TagId),
            TagsSort.IdDesc => query.OrderByDescending(t => t.TagId),
            _ => query
        };

        // Get total count
        var totalCount = await query.CountAsync(ct);

        // Pagination
        var skip = (request.Page - 1) * request.PageSize;
        query = query
            .Skip(skip)
            .Take(request.PageSize);

        var items = await query
            .Select(t => new TagCMSDto(
               t.TagId,
               t.TitleEn,
               t.TitleJp,
               t.CreatedAt,
               t.UpdatedAt
        )).ToListAsync(ct);

        return (items, totalCount);
    }

    #endregion

    #region Tags Dropdown

    public async Task<IReadOnlyList<TagDto>> GetAllTagsDropdownAsync(CancellationToken ct)
    {
        return await _db.Tags
            .AsNoTracking()
            .Select(t => new TagDto(
               t.TagId,
               t.TitleEn,
               t.TitleJp
        )).ToListAsync(ct);
    }

    #endregion

    #region Create Tag

    public async Task CreateTagAsync(TagRequest request, CancellationToken ct)
    {
        // Validate Request
        if(string.IsNullOrWhiteSpace(request.TitleEn)) 
            throw new BadRequestException("Tag must have English title to be created");

        // Normalize Input 
        var titleEn = request.TitleEn.Trim();
        var titleJp = string.IsNullOrWhiteSpace(request.TitleJp) ? null : request.TitleJp.Trim();

        // Validate no duplicate tags
        var exists = await _db.Tags.AnyAsync(t =>
            t.TitleEn.ToLower() == titleEn.ToLower() ||
            (titleJp != null && t.TitleJp != null && t.TitleJp.ToLower() == titleJp.ToLower()),
            ct
        );
        if (exists)
            throw new BadRequestException("A tag with the same English or Japanese title already exists.");

        // Create Tag
        var tag = new Tag
        {
            TitleEn = titleEn,
            TitleJp = titleJp
        };

        _db.Tags.Add(tag);
        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region Update Tag

    public async Task UpdateTagAsync(int tagId, TagRequest request, CancellationToken ct)
    {
        // Validate Tag exists
        var tag = await _db.Tags.FirstOrDefaultAsync(t => t.TagId == tagId, ct);
        if (tag is null) throw new NotFoundException($"Tag {tagId} was not found.");

        // Validate request
        if(string.IsNullOrWhiteSpace(request.TitleEn)) 
            throw new BadRequestException("Tag must have English title");

        // Normalize Input 
        var titleEn = request.TitleEn.Trim();
        var titleJp = string.IsNullOrWhiteSpace(request.TitleJp) ? null : request.TitleJp.Trim();

        // Validate no duplicate tags (excluding current tag)
        var exists = await _db.Tags.AnyAsync(t =>
            t.TagId != tagId &&
            (
                t.TitleEn.ToLower() == titleEn.ToLower() ||
                (titleJp != null && t.TitleJp != null && t.TitleJp.ToLower() == titleJp.ToLower())
            ),
            ct
        );
        if (exists)
            throw new BadRequestException("A tag with the same English or Japanese title already exists.");

        // Update Tag
        tag.TitleEn = titleEn;
        tag.TitleJp = titleJp;

        await _db.SaveChangesAsync(ct);
    }

    #endregion

    #region Delete Tag

    public async Task DeleteTagAsync(int tagId, CancellationToken ct)
    {
        // Validate Tag exists
        var tag = await _db.Tags.FirstOrDefaultAsync(t => t.TagId == tagId, ct);
        if (tag is null) throw new NotFoundException($"Tag {tagId} was not found.");

        _db.Tags.Remove(tag);
        await _db.SaveChangesAsync(ct);
    }

    #endregion
}
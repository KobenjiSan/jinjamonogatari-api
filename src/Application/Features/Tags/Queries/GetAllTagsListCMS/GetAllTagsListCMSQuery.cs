using Application.Features.Tags.Models;
using Domain.Enums;
using MediatR;

namespace Application.Features.Tags.Queries.GetAllTagsListCMS;

// QUERIES
public record GetAllTagsListCMSQuery(
    string? SearchQuery,
    TagsSort? Sort,
    int Page = 1,
    int PageSize = 5
) : IRequest<GetAllTagsListCMSResult>;

// RESULTS
public record GetAllTagsListCMSResult(IReadOnlyList<TagCMSDto> Tags, int TotalCount);
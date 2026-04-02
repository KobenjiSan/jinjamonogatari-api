using Application.Features.Shrines.Models;
using Domain.Enums;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineListCMS;

// QUERIES
public record GetShrineListCMSQuery(
    string? Status,
    string? Prefecture,
    string? SearchQuery,
    ShrineSort? Sort,
    int Page = 1,
    int PageSize = 5
) : IRequest<GetShrineListCMSResult>;

// RESULTS
public record GetShrineListCMSResult(IReadOnlyList<ShrineListCMSDto> shrines, int totalCount);
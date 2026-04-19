using Application.Features.Shrines.Models;
using Application.Features.Tags.Models;
using Domain.Enums;
using MediatR;

namespace Application.Features.Kami.Queries.GetAllKamiCMS;

// QUERIES
public record GetAllKamiCMSQuery(
    string? SearchQuery,
    TagsSort? Sort,
    int Page = 1,
    int PageSize = 5
) : IRequest<GetAllKamiCMSResult>;

// RESULTS
public record GetAllKamiCMSResult(IReadOnlyList<KamiReadCMSDto> Kami, int TotalCount);
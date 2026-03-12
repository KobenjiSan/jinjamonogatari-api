using Application.Features.Shrines.Models;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineKamiByIdCMS;

// QUERIES
public record GetShrineKamiByIdCMSQuery(int Id) : IRequest<GetShrineKamiByIdCMSResult>;

// RESULTS
public record GetShrineKamiByIdCMSResult(IReadOnlyList<KamiReadCMSDto> Kami);
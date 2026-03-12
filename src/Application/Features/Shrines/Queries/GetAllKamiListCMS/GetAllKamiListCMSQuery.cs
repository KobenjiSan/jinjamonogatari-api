using Application.Features.Shrines.Models;
using MediatR;

namespace Application.Features.Shrines.Queries.GetAllKamiListCMS;

// QUERIES
public record GetAllKamiListCMSQuery() : IRequest<GetAllKamiListCMSResult>;

// RESULTS
public record GetAllKamiListCMSResult(IReadOnlyList<KamiReadCMSDto> Kami);
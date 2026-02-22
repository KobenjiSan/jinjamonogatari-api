using Application.Features.Shrines.Models;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineKamiBySlug;

// QUERIES
public record GetShrineKamiBySlugQuery(string Slug) : IRequest<GetShrineKamiBySlugResult>;

// RESULTS
public record GetShrineKamiBySlugResult(IReadOnlyList<KamiReadDto> Kami);
using Application.Features.Shrines.Models;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineMetaBySlug;

// QUERIES
public record GetShrineMetaBySlugQuery(string Slug) : IRequest<GetShrineMetaBySlugResult>;

// RESULTS
public record GetShrineMetaBySlugResult(ShrineMetaDto Meta);
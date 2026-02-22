using Application.Features.Shrines.Models;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineGalleryBySlug;

// QUERIES
public record GetShrineGalleryBySlugQuery(string Slug) : IRequest<GetShrineGalleryBySlugResult>;

// RESULTS
public record GetShrineGalleryBySlugResult(IReadOnlyList<GalleryListItemDto> Images);
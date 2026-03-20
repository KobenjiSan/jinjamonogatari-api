using Application.Common.Models.Images;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineGalleryByIdCMS;

// QUERIES
public record GetShrineGalleryByIdCMSQuery(int Id) : IRequest<GetShrineGalleryByIdCMSResult>;

// RESULTS
public record GetShrineGalleryByIdCMSResult(IReadOnlyList<ImageCMSDto> Images);
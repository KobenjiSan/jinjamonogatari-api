using Application.Common.Models.Images;
using MediatR;

namespace Application.Features.Shrines.Queries.GetImageById;

// QUERIES
public record GetImageByIdQuery(int Id) : IRequest<GetImageByIdResult>;

// RESULTS
public record GetImageByIdResult(ImageFullDto Image);
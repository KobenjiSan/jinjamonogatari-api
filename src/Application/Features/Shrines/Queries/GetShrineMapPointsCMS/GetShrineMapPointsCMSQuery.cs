using Application.Features.Shrines.Models;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineMapPointsCMS;

// QUERIES
public record GetShrineMapPointsCMSQuery : IRequest<GetShrineMapPointsCMSResult>;

// RESULTS
public record GetShrineMapPointsCMSResult(IReadOnlyList<ShrineMapPointCMSDto> MapPoints);
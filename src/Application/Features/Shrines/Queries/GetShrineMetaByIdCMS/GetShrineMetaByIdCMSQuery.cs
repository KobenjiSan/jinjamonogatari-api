using Application.Features.Shrines.Models;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineMetaByIdCMS;

// QUERIES
public record GetShrineMetaByIdCMSQuery(int Id) : IRequest<GetShrineMetaByIdCMSResult>;

// RESULTS
public record GetShrineMetaByIdCMSResult(ShrineMetaCMSDto Meta);
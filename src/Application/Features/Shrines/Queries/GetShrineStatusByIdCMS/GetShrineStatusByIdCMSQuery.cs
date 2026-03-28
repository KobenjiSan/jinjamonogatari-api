using Application.Features.Shrines.Models;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineStatusByIdCMS;

// QUERIES
public record GetShrineStatusByIdCMSQuery(int Id) : IRequest<GetShrineStatusByIdCMSResult>;

// RESULTS
public record GetShrineStatusByIdCMSResult(string Status);
using Application.Features.Shrines.Models;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineNotesByIdCMS;

// QUERIES
public record GetShrineNotesByIdCMSQuery(int Id) : IRequest<GetShrineNotesByIdCMSResult>;

// RESULTS
public record GetShrineNotesByIdCMSResult(string Notes);
using Application.Features.Shrines.Models;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineFolkloreByIdCMS;

// QUERIES
public record GetShrineFolkloreByIdCMSQuery(int Id) : IRequest<GetShrineFolkloreByIdCMSResult>;

// RESULTS
public record GetShrineFolkloreByIdCMSResult(IReadOnlyList<FolkloreReadCMSDto> Folklore);
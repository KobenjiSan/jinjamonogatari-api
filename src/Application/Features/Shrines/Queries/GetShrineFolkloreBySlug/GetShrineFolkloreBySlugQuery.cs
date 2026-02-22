using Application.Features.Shrines.Models;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrineFolkloreBySlug;

// QUERIES
public record GetShrineFolkloreBySlugQuery(string Slug) : IRequest<GetShrineFolkloreBySlugResult>;

// RESULTS
public record GetShrineFolkloreBySlugResult(IReadOnlyList<FolkloreReadDto> Folklore);
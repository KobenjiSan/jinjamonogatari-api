using Application.Features.Shrines.Models;
using MediatR;

namespace Application.Features.Shrines.Queries.GetAllTagsListCMS;

// QUERIES
public record GetAllTagsListCMSQuery() : IRequest<GetAllTagsListCMSResult>;

// RESULTS
public record GetAllTagsListCMSResult(IReadOnlyList<TagDto> Tags);
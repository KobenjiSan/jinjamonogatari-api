using Application.Features.Shrines.Models;
using MediatR;

namespace Application.Features.Tags.Queries.GetAllTagsDropdown;

// QUERIES
public record GetAllTagsDropdownQuery() : IRequest<GetAllTagsDropdownResult>;

// RESULTS
public record GetAllTagsDropdownResult(IReadOnlyList<TagDto> Tags);
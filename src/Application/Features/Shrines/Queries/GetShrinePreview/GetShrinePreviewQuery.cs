using Application.Features.Shrines.Models;
using MediatR;

namespace Application.Features.Shrines.Queries.GetShrinePreview;

// QUERIES
public record GetShrinePreviewQuery(string Slug) : IRequest<GetShrinePreviewResult>;

// RESULTS
public record GetShrinePreviewResult(ShrinePreviewDto Preview);
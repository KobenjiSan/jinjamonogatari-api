using Application.Features.Shrines.Models;
using MediatR;

namespace Application.Features.Shrines.Queries.GetImportPreviewCMS;

// QUERIES
public record GetImportPreviewCMSQuery(ImportPreviewRequest Request) : IRequest<GetImportPreviewCMSResult>;

// RESULTS
public record GetImportPreviewCMSResult(IReadOnlyList<ImportPreviewItemDto> Preview);
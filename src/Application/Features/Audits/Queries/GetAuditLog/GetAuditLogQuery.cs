using Application.Features.Audits.Models;
using Domain.Enums;
using MediatR;

namespace Application.Features.Audits.Queries.GetAuditLog;

// QUERIES
public record GetAuditLogQuery(
    string? SearchQuery,
    AuditSort? Sort,
    int Page = 1,
    int PageSize = 5
) : IRequest<GetAuditLogResult>;

// RESULTS
public record GetAuditLogResult(IReadOnlyList<AuditDto> Audits, int TotalCount);
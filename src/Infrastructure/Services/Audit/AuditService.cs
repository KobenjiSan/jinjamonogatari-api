using Application.Features.Audits.Models;
using Application.Features.Audits.Queries.GetAuditLog;
using Application.Features.Audits.Services;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Audit;

public class AuditService : IAuditService
{
    private readonly AppDbContext _db;

    public AuditService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<(IReadOnlyList<AuditDto>, int)> GetAuditLogAsync(GetAuditLogQuery request, CancellationToken ct)
    {
        var query = _db.AuditLogs.AsNoTracking();

        // Search Query
        if(!string.IsNullOrWhiteSpace(request.SearchQuery))
        {
            var search = request.SearchQuery.Trim().ToLower();

            query = query.Where(a =>
                (a.Username != null && a.Username.ToLower().Contains(search)) ||
                (a.Action != null && a.Action.ToLower().Contains(search)) || 
                (a.Message != null && a.Message.ToLower().Contains(search)) || 
                (a.Target != null && a.Target.ToLower().Contains(search))
            );
        }

        // Sort Type / Direction
        var sort = request.Sort ?? AuditSort.CreatedDesc;
        query = sort switch
        {
            AuditSort.CreatedAsc => query.OrderBy(t => t.CreatedAt),
            AuditSort.CreatedDesc => query.OrderByDescending(t => t.CreatedAt),
            AuditSort.UserAsc => query.OrderBy(t => t.UserId),
            AuditSort.UserDesc => query.OrderByDescending(t => t.UserId),
            AuditSort.ActionAsc => query.OrderBy(t => t.Action),
            AuditSort.ActionDesc => query.OrderByDescending(t => t.Action),
            AuditSort.SuccessAsc => query.OrderBy(t => t.IsSuccessful),
            AuditSort.SuccessDesc => query.OrderByDescending(t => t.IsSuccessful),
            _ => query
        };

        // Get total count
        var totalCount = await query.CountAsync(ct);

        // Pagination
        var skip = (request.Page - 1) * request.PageSize;
        query = query
            .Skip(skip)
            .Take(request.PageSize);

        var items = await query
            .Select(a => new AuditDto(
               a.AuditId,
               a.UserId,
               a.Username,
               a.Action,
               a.Target,
               a.IsSuccessful,
               a.Message,
               a.CreatedAt
        )).ToListAsync(ct);

        return (items, totalCount);
    }

    public async Task LogAsync(int userId, string username, string action, string target, bool isSuccessful, string? message, CancellationToken ct)
    {
        var audit = new AuditLog
        {
            UserId = userId,
            Username = username,
            Action = action,
            Target = target,
            IsSuccessful = isSuccessful,
            Message = message,
        };

        _db.AuditLogs.Add(audit);
        await _db.SaveChangesAsync(ct);
    }
}
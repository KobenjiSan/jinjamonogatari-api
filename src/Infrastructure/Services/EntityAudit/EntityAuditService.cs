using Application.Common.Exceptions;
using Application.Common.Models.EntityAudit;
using Application.Common.Services;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.EntityAudit;

public class EntityAuditService : IEntityAuditService
{
    private readonly AppDbContext _db;

    public EntityAuditService(AppDbContext db)
    {
        _db = db;
    }

    #region Audit Kami

    public async Task<EntityAuditCMSDto> AuditKamiAsync(int kamiId, CancellationToken ct)
    {
        var 今 = DateTime.UtcNow;

        var kami = await _db.Kamis
            .Include(k => k.EntityAudit)
                .ThenInclude(a => a!.Issues)
            .FirstOrDefaultAsync(k => k.KamiId == kamiId, ct);
        if (kami == null)
            throw new NotFoundException("Kami not found.");

        // get kami snapshot
        var snapshot = await GetKamiAuditSnapshotAsync(kamiId, ct);
        if (snapshot == null)
            throw new NotFoundException("Kami Audit could not be completed. Snapshot failed to load.");

        // get kami's head audit
        var headAudit = kami.EntityAudit;

        // create blank headAudit if null
        if (headAudit == null)
        {
            headAudit = new Domain.Entities.EntityAudit
            {
                CreatedAt = 今,
                UpdatedAt = 今,
                Issues = []
            };

            kami.EntityAudit = headAudit;
        }

        // create new issues while evaluating in memory
        var issues = new List<EntityAuditIssueDraft>();
        EvaluateKami(issues, snapshot);

        // delete old issues 
        _db.Set<EntityAuditIssue>().RemoveRange(headAudit.Issues);
        headAudit.Issues.Clear();

        // Create new issues
        foreach (var issue in issues)
        {
            headAudit.Issues.Add(new EntityAuditIssue
            {
                Severity = issue.Severity,
                Field = issue.Field,
                Message = issue.Message,
                RelatedItemType = issue.RelatedItemType,
                RelatedItemId = issue.RelatedItemId,
                CreatedAt = 今
            });
        }

        // Update head audit based on new issues
        var errorCount = issues.Count(x => x.Severity == "Error");
        var warningCount = issues.Count(x => x.Severity == "Warning");

        headAudit.ErrorCount = errorCount;
        headAudit.WarningCount = warningCount;
        headAudit.CanSubmit = errorCount == 0;
        headAudit.UpdatedAt = 今;

        // Save everything together        
        await _db.SaveChangesAsync(ct);

        // return audit
        return new EntityAuditCMSDto(
            EntityAuditId: headAudit.EntityAuditId,
            ErrorCount: headAudit.ErrorCount,
            WarningCount: headAudit.WarningCount,
            CanSubmit: headAudit.CanSubmit,
            Issues: headAudit.Issues
                .Select(issue => new EntityAuditIssueDto(
                    EntityAuditIssueId: issue.EntityAuditIssueId,
                    EntityAuditId: headAudit.EntityAuditId,
                    Severity: issue.Severity,
                    Field: issue.Field,
                    Message: issue.Message,
                    RelatedItemType: issue.RelatedItemType,
                    RelatedItemId: issue.RelatedItemId,
                    CreatedAt: issue.CreatedAt
                ))
                .ToList(),
            CreatedAt: headAudit.CreatedAt,
            UpdatedAt: headAudit.UpdatedAt
        );
    }

    public async Task<KamiAuditSnapshot?> GetKamiAuditSnapshotAsync(int kamiId, CancellationToken ct)
    {
        return await _db.Kamis
            .AsNoTracking()
            .Where(k => k.KamiId == kamiId)
            .Select(k => new KamiAuditSnapshot
            {
                KamiId = k.KamiId,
                NameEn = k.NameEn,
                NameJp = k.NameJp,
                Desc = k.Desc,
                Image = k.Image == null ? null : new ImageAuditSnapshot
                {
                    ImgId = k.Image.ImgId,
                    ImageUrl = k.Image.ImageUrl,
                    Title = k.Image.Title,
                    Desc = k.Image.Desc,
                    Citation = k.Image.Citation == null ? null : new CitationAuditSnapshot
                    {
                        CiteId = k.Image.Citation.CiteId,
                        Title = k.Image.Citation.Title,
                        Author = k.Image.Citation.Author,
                        Url = k.Image.Citation.Url,
                        Year = k.Image.Citation.Year
                    }
                },
                Citations = k.KamiCitations
                    .Select(kc => new CitationAuditSnapshot
                    {
                        CiteId = kc.Citation.CiteId,
                        Title = kc.Citation.Title,
                        Author = kc.Citation.Author,
                        Url = kc.Citation.Url,
                        Year = kc.Citation.Year
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(ct);
    }

    private static void EvaluateKami(List<EntityAuditIssueDraft> issues, KamiAuditSnapshot snapshot)
    {
        if (string.IsNullOrWhiteSpace(snapshot.NameEn))
        {
            AddError(issues, null, null, "NameEn", "Kami is missing English name.");
        }

        if (string.IsNullOrWhiteSpace(snapshot.NameJp))
        {
            AddError(issues, null, null, "NameJp", "Kami is missing Japanese name.");
        }

        if (string.IsNullOrWhiteSpace(snapshot.Desc))
        {
            AddError(issues, null, null, "Desc", "Kami is missing description.");
        }

        if (snapshot.Citations.Count == 0)
        {
            AddError(issues, null, null, "Citations", "Kami must have at least one citation.");
        }
        else
        {
            foreach (var citation in snapshot.Citations)
            {
                IsCitationValid(citation, issues, "Citation", citation.CiteId);
            }
        }

        if (snapshot.Image is null)
        {
            AddWarning(issues, null, null, "Image", "Kami image is preferred.");
        }
        else
        {
            AuditImage(snapshot.Image, issues);
        }
    }

    #endregion

    #region Image

    private static bool AuditImage(
        ImageAuditSnapshot image,
        List<EntityAuditIssueDraft> issues)
    {
        var isValid = true;

        if (string.IsNullOrWhiteSpace(image.ImageUrl))
        {
            AddError(issues, "Image", image.ImgId, "ImageUrl", "Image is missing source.");
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(image.Title))
        {
            AddError(issues, "Image", image.ImgId, "Title", "Image is missing title.");
            isValid = false;
        }

        if (image.Citation is null)
        {
            AddError(issues, "Image", image.ImgId, "Citation", "Image must have a citation.");
            isValid = false;
        }
        else
        {
            if (!IsCitationValid(image.Citation, issues, "Image Citation", image.Citation.CiteId))
                isValid = false;
        }

        return isValid;
    }

    #endregion

    #region Citation

    private static bool IsCitationValid(
        CitationAuditSnapshot citation,
        List<EntityAuditIssueDraft> issues,
        string section,
        int? itemId)
    {
        var isValid = true;

        if (string.IsNullOrWhiteSpace(citation.Title))
        {
            AddError(issues, section, itemId, "Title", "Citation is missing title.");
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(citation.Author))
        {
            AddError(issues, section, itemId, "Author", "Citation is missing author.");
            isValid = false;
        }

        if (!citation.Year.HasValue)
        {
            AddError(issues, section, itemId, "Year", "Citation is missing year.");
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(citation.Url))
        {
            AddWarning(issues, section, itemId, "Url", "Citation URL is suggested.");
        }
        else if (!Uri.TryCreate(citation.Url, UriKind.Absolute, out _))
        {
            AddError(issues, section, itemId, "Url", "Citation URL format is invalid.");
            isValid = false;
        }

        return isValid;
    }

    #endregion

    #region Add Error

    private static void AddError(List<EntityAuditIssueDraft> issues, string? relatedItemType, int? relatedItemId, string field, string message)
    {
        issues.Add(new EntityAuditIssueDraft(
            Severity: "Error",
            Field: field,
            Message: message,
            RelatedItemType: relatedItemType,
            RelatedItemId: relatedItemId
        ));
    }

    #endregion

    #region Add Warning

    private static void AddWarning(List<EntityAuditIssueDraft> issues, string? relatedItemType, int? relatedItemId, string field, string message)
    {
        issues.Add(new EntityAuditIssueDraft(
            Severity: "Warning",
            Field: field,
            Message: message,
            RelatedItemType: relatedItemType,
            RelatedItemId: relatedItemId
        ));
    }

    #endregion
}
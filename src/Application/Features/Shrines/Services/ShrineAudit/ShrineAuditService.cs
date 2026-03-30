using System.Text.RegularExpressions;
using Application.Features.Shrines.Models;

namespace Application.Features.Shrines.Services.ShrineAudit;

public class ShrineAuditService : IShrineAuditService
{
    #region Evaluate

    public ShrineAuditDto Evaluate(ShrineAuditSnapshot snapshot)
    {
        var issues = new List<AuditIssueDto>();

        AuditShrineMeta(snapshot, issues);
        AuditHeroImage(snapshot, issues);
        AuditTags(snapshot, issues);
        AuditKami(snapshot, issues);
        AuditHistories(snapshot, issues);
        AuditFolklores(snapshot, issues);
        AuditGallery(snapshot, issues);

        return new ShrineAuditDto
        {
            ShrineId = snapshot.ShrineId,
            Issues = issues,
            ErrorCount = issues.Count(x => x.Severity == "Error"),
            WarningCount = issues.Count(x => x.Severity == "Warning"),
            IsSubmittable = issues.All(x => x.Severity != "Error")
        };
    }

    #endregion

    #region Evaluate Kami

    public EntityAuditDto EvaluateKami(KamiAuditSnapshot kami)
    {
        var issues = new List<AuditIssueDto>();

        if (string.IsNullOrWhiteSpace(kami.NameEn))
        {
            AddError(issues, "Kami", kami.KamiId, "Kami.NameEn", "Kami is missing English name.");
        }

        if (string.IsNullOrWhiteSpace(kami.NameJp))
        {
            AddError(issues, "Kami", kami.KamiId, "Kami.NameJp", "Kami is missing Japanese name.");
        }

        if (string.IsNullOrWhiteSpace(kami.Desc))
        {
            AddError(issues, "Kami", kami.KamiId, "Kami.Desc", "Kami is missing description.");
        }

        if (kami.Citations.Count == 0)
        {
            AddError(issues, "Kami", kami.KamiId, "Kami.Citations", "Kami must have at least one citation.");
        }
        else
        {
            foreach (var citation in kami.Citations)
            {
                IsCitationValid(citation, issues, "Kami", kami.KamiId, "Kami.Citation");
            }
        }

        if (kami.Image is null)
        {
            AddWarning(issues, "Kami", kami.KamiId, "Kami.Image", "Kami image is preferred.");
        }
        else
        {
            AuditImage(kami.Image, issues, "Kami", kami.KamiId, "Kami.Image");
        }

        var errorCount = issues.Count(x => x.Severity == "Error");
        var warningCount = issues.Count(x => x.Severity == "Warning");

        return new EntityAuditDto(
            kami.KamiId,
            errorCount,
            warningCount,
            errorCount > 0,
            warningCount > 0,
            issues
        );
    }

    #endregion

    #region Evaluate Folklore

    public EntityAuditDto EvaluateFolklore(FolkloreAuditSnapshot folklore)
    {
        var issues = new List<AuditIssueDto>();

        if (string.IsNullOrWhiteSpace(folklore.Title))
        {
            AddError(issues, "Folklore", folklore.FolkloreId, "Folklore.Title", "Folklore is missing title.");
        }

        if (string.IsNullOrWhiteSpace(folklore.Information))
        {
            AddError(issues, "Folklore", folklore.FolkloreId, "Folklore.Information", "Folklore is missing story.");
        }

        if (folklore.SortOrder.HasValue && folklore.SortOrder.Value <= 0)
        {
            AddError(issues, "Folklore", folklore.FolkloreId, "Folklore.SortOrder", "Folklore sort order must be positive.");
        }

        if (folklore.Citations.Count == 0)
        {
            AddError(issues, "Folklore", folklore.FolkloreId, "Folklore.Citations", "Folklore must have at least one citation.");
        }
        else
        {
            foreach (var citation in folklore.Citations)
            {
                IsCitationValid(citation, issues, "Folklore", folklore.FolkloreId, "Folklore.Citation");
            }
        }

        if (folklore.Image is null)
        {
            AddWarning(issues, "Folklore", folklore.FolkloreId, "Folklore.Image", "Folklore image is preferred.");
        }
        else
        {
            AuditImage(folklore.Image, issues, "Folklore", folklore.FolkloreId, "Folklore.Image");
        }

        var errorCount = issues.Count(x => x.Severity == "Error");
        var warningCount = issues.Count(x => x.Severity == "Warning");

        return new EntityAuditDto(
            folklore.FolkloreId,
            errorCount,
            warningCount,
            errorCount > 0,
            warningCount > 0,
            issues
        );
    }

    #endregion

    #region Evaluate History

    public EntityAuditDto EvaluateHistory(HistoryAuditSnapshot history)
    {
        var issues = new List<AuditIssueDto>();

        if (!history.EventDate.HasValue)
        {
            AddError(issues, "History", history.HistoryId, "History.EventDate", "History is missing date.");
        }

        if (string.IsNullOrWhiteSpace(history.Title))
        {
            AddError(issues, "History", history.HistoryId, "History.Title", "History is missing title.");
        }

        if (string.IsNullOrWhiteSpace(history.Information))
        {
            AddError(issues, "History", history.HistoryId, "History.Information", "History is missing information.");
        }

        if (!history.SortOrder.HasValue)
        {
            AddError(issues, "History", history.HistoryId, "History.SortOrder", "History is missing sort order.");
        }
        else if (history.SortOrder.Value <= 0)
        {
            AddError(issues, "History", history.HistoryId, "History.SortOrder", "History sort order must be positive.");
        }

        if (history.Citations.Count == 0)
        {
            AddError(issues, "History", history.HistoryId, "History.Citations", "History must have at least one citation.");
        }
        else
        {
            foreach (var citation in history.Citations)
            {
                IsCitationValid(citation, issues, "History", history.HistoryId, "History.Citation");
            }
        }

        if (history.Image is not null)
        {
            AuditImage(history.Image, issues, "History", history.HistoryId, "History.Image");
        }

        var errorCount = issues.Count(x => x.Severity == "Error");
        var warningCount = issues.Count(x => x.Severity == "Warning");

        return new EntityAuditDto(
            history.HistoryId,
            errorCount,
            warningCount,
            errorCount > 0,
            warningCount > 0,
            issues
        );
    }
    
    #endregion

    #region Evaluate Gallery

    public EntityAuditDto EvaluateGalleryImage(ImageAuditSnapshot image)
    {
        var issues = new List<AuditIssueDto>();

        AuditImage(image, issues, "Gallery", image.ImgId, "GalleryImage");

        var errorCount = issues.Count(x => x.Severity == "Error");
        var warningCount = issues.Count(x => x.Severity == "Warning");

        return new EntityAuditDto(
            image.ImgId,
            errorCount,
            warningCount,
            errorCount > 0,
            warningCount > 0,
            issues
        );
    }

    #endregion

    #region Shrine Meta

    private static void AuditShrineMeta(ShrineAuditSnapshot shrine, List<AuditIssueDto> issues)
    {
        if (string.IsNullOrWhiteSpace(shrine.Slug))
        {
            AddError(issues, "ShrineMeta", null, "ShrineMeta.Slug", "Shrine is missing slug.");
        }
        else if (!Regex.IsMatch(shrine.Slug, "^[a-z0-9-]+$"))
        {
            AddError(issues, "ShrineMeta", null, "ShrineMeta.Slug", "Slug must contain only lowercase letters, numbers, and hyphens.");
        }

        if (string.IsNullOrWhiteSpace(shrine.NameEn))
            AddError(issues, "ShrineMeta", null, "ShrineMeta.NameEn", "Shrine is missing English name.");

        if (string.IsNullOrWhiteSpace(shrine.NameJp))
            AddError(issues, "ShrineMeta", null, "ShrineMeta.NameJp", "Shrine is missing Japanese name.");

        if (string.IsNullOrWhiteSpace(shrine.ShrineDesc))
            AddError(issues, "ShrineMeta", null, "ShrineMeta.ShrineDesc", "Shrine is missing description.");

        if (!shrine.Lat.HasValue)
            AddError(issues, "ShrineMeta", null, "ShrineMeta.Lat", "Shrine is missing latitude.");

        if (!shrine.Lon.HasValue)
            AddError(issues, "ShrineMeta", null, "ShrineMeta.Lon", "Shrine is missing longitude.");

        if (!string.IsNullOrWhiteSpace(shrine.Email) && !IsValidEmail(shrine.Email))
            AddError(issues, "ShrineMeta", null, "ShrineMeta.Email", "Shrine email is invalid.");

        if (!string.IsNullOrWhiteSpace(shrine.Website) && !Uri.TryCreate(shrine.Website, UriKind.Absolute, out _))
            AddError(issues, "ShrineMeta", null, "ShrineMeta.Website", "Shrine website URL is invalid.");

        if (!string.IsNullOrWhiteSpace(shrine.PhoneNumber) && !IsValidPhone(shrine.PhoneNumber))
            AddError(issues, "ShrineMeta", null, "ShrineMeta.PhoneNumber", "Shrine phone number is invalid.");

        var addressFields = new[]
        {
            shrine.Prefecture,
            shrine.City,
            shrine.Ward,
            shrine.Locality,
            shrine.PostalCode,
            shrine.Country
        };

        var anyAddressFilled = addressFields.Any(x => !string.IsNullOrWhiteSpace(x));
        var allAddressFilled = addressFields.All(x => !string.IsNullOrWhiteSpace(x));

        if (anyAddressFilled && !allAddressFilled)
            AddError(issues, "ShrineMeta", null, "ShrineMeta.Address", "Address cannot be partially complete.");
    }

    #endregion

    #region Hero Image

    private static void AuditHeroImage(ShrineAuditSnapshot shrine, List<AuditIssueDto> issues)
    {
        if (shrine.HeroImage is null)
        {
            AddError(issues, "ShrineMeta", null, "HeroImage", "Shrine must have a hero image.");
            return;
        }

        AuditImage(
            shrine.HeroImage,
            issues,
            section: "ShrineMeta",
            itemId: shrine.HeroImage.ImgId,
            fieldPrefix: "HeroImage");
    }

    #endregion

    #region Tags

    private static void AuditTags(ShrineAuditSnapshot shrine, List<AuditIssueDto> issues)
    {
        if (shrine.Tags.Count == 0)
            AddError(issues, "ShrineMeta", null, "ShrineMeta.Tags", "Shrine must have at least one tag.");

        foreach (var tag in shrine.Tags)
        {
            if (string.IsNullOrWhiteSpace(tag.TitleEn))
            {
                AddError(issues, "ShrineMeta", tag.TagId, "Tag.TitleEn", "Tag is missing English title.");
            }

            if (string.IsNullOrWhiteSpace(tag.TitleJp))
            {
                AddError(issues, "ShrineMeta", tag.TagId, "Tag.TitleJp", "Tag is missing Japanese title.");
            }
        }
    }

    #endregion

    #region Kami

    private static void AuditKami(ShrineAuditSnapshot shrine, List<AuditIssueDto> issues)
    {
        var fullKamiCount = 0;

        foreach (var kami in shrine.Kami)
        {
            var isFull = true;

            if (string.IsNullOrWhiteSpace(kami.NameEn))
            {
                AddError(issues, "Kami", kami.KamiId, "Kami.NameEn", "Kami is missing English name.");
                isFull = false;
            }

            if (string.IsNullOrWhiteSpace(kami.NameJp))
            {
                AddError(issues, "Kami", kami.KamiId, "Kami.NameJp", "Kami is missing Japanese name.");
                isFull = false;
            }

            if (string.IsNullOrWhiteSpace(kami.Desc))
            {
                AddError(issues, "Kami", kami.KamiId, "Kami.Desc", "Kami is missing description.");
                isFull = false;
            }

            if (kami.Citations.Count == 0)
            {
                AddError(issues, "Kami", kami.KamiId, "Kami.Citations", "Kami must have at least one citation.");
                isFull = false;
            }
            else
            {
                foreach (var citation in kami.Citations)
                {
                    if (!IsCitationValid(citation, issues, "Kami", kami.KamiId, "Kami.Citation"))
                        isFull = false;
                }
            }

            if (kami.Image is null)
            {
                AddWarning(issues, "Kami", kami.KamiId, "Kami.Image", "Kami image is preferred.");
            }
            else
            {
                if (!AuditImage(kami.Image, issues, "Kami", kami.KamiId, "Kami.Image"))
                    isFull = false;
            }

            if (isFull)
                fullKamiCount++;
        }

        if (fullKamiCount == 0)
        {
            AddError(issues, "Kami", null, "Kami", "Shrine must have at least one full kami.");   
        }
    }

    #endregion

    #region History

    private static void AuditHistories(ShrineAuditSnapshot shrine, List<AuditIssueDto> issues)
    {
        var fullHistoryCount = 0;

        var duplicateSortOrders = shrine.Histories
            .Where(h => h.SortOrder.HasValue)
            .GroupBy(h => h.SortOrder!.Value)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToHashSet();

        foreach (var history in shrine.Histories)
        {
            var isFull = true;

            if (!history.EventDate.HasValue)
            {
                AddError(issues, "History", history.HistoryId, "History.EventDate", "History is missing date.");
                isFull = false;
            }

            if (string.IsNullOrWhiteSpace(history.Title))
            {
                AddError(issues, "History", history.HistoryId, "History.Title", "History is missing title.");
                isFull = false;
            }

            if (string.IsNullOrWhiteSpace(history.Information))
            {
                AddError(issues, "History", history.HistoryId, "History.Information", "History is missing information.");
                isFull = false;
            }

            if (!history.SortOrder.HasValue)
            {
                AddError(issues, "History", history.HistoryId, "History.SortOrder", "History is missing sort order.");
                isFull = false;
            }
            else
            {
                if (history.SortOrder.Value <= 0)
                {
                    AddError(issues, "History", history.HistoryId, "History.SortOrder", "History sort order must be positive.");
                    isFull = false;
                }

                if (duplicateSortOrders.Contains(history.SortOrder.Value))
                {
                    AddError(issues, "History", history.HistoryId, "History.SortOrder", "History sort order must be unique within the shrine.");
                    isFull = false;
                }
            }

            if (history.Citations.Count == 0)
            {
                AddError(issues, "History", history.HistoryId, "History.Citations", "History must have at least one citation.");
                isFull = false;
            }
            else
            {
                foreach (var citation in history.Citations)
                {
                    if (!IsCitationValid(citation, issues, "History", history.HistoryId, "History.Citation"))
                        isFull = false;
                }
            }

            if (history.Image is not null)
            {
                if (!AuditImage(history.Image, issues, "History", history.HistoryId, "History.Image"))
                    isFull = false;
            }

            if (isFull)
                fullHistoryCount++;
        }

        if (fullHistoryCount == 0)
        {
            AddError(issues, "History", null, "History", "Shrine must have at least one full history.");
        }
    }

    #endregion

    #region Folklore

    private static void AuditFolklores(ShrineAuditSnapshot shrine, List<AuditIssueDto> issues)
    {
        var fullFolkloreCount = 0;

        var duplicateSortOrders = shrine.Folklores
            .Where(f => f.SortOrder.HasValue)
            .GroupBy(f => f.SortOrder!.Value)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToHashSet();

        foreach (var folklore in shrine.Folklores)
        {
            var isFull = true;

            if (string.IsNullOrWhiteSpace(folklore.Title))
            {
                AddError(issues, "Folklore", folklore.FolkloreId, "Folklore.Title", "Folklore is missing title.");
                isFull = false;
            }

            if (string.IsNullOrWhiteSpace(folklore.Information))
            {
                AddError(issues, "Folklore", folklore.FolkloreId, "Folklore.Information", "Folklore is missing story.");
                isFull = false;
            }

            if (folklore.SortOrder.HasValue)
            {
                if (folklore.SortOrder.Value <= 0)
                {
                    AddError(issues, "Folklore", folklore.FolkloreId, "Folklore.SortOrder", "Folklore sort order must be positive.");
                    isFull = false;
                }

                if (duplicateSortOrders.Contains(folklore.SortOrder.Value))
                {
                    AddError(issues, "Folklore", folklore.FolkloreId, "Folklore.SortOrder", "Folklore sort order must be unique within the shrine.");
                    isFull = false;
                }
            }

            if (folklore.Citations.Count == 0)
            {
                AddError(issues, "Folklore", folklore.FolkloreId, "Folklore.Citations", "Folklore must have at least one citation.");
                isFull = false;
            }
            else
            {
                foreach (var citation in folklore.Citations)
                {
                    if (!IsCitationValid(citation, issues, "Folklore", folklore.FolkloreId, "Folklore.Citation"))
                        isFull = false;
                }
            }

            if (folklore.Image is null)
            {
                AddWarning(issues, "Folklore", folklore.FolkloreId, "Folklore.Image", "Folklore image is preferred.");
            }
            else
            {
                if (!AuditImage(folklore.Image, issues, "Folklore", folklore.FolkloreId, "Folklore.Image"))
                    isFull = false;
            }

            if (isFull)
                fullFolkloreCount++;
        }

        if (fullFolkloreCount == 0) 
        {
            AddError(issues, "Folklore", null, "Folklore", "Shrine must have at least one full folklore.");
        }
    }

    #endregion

    #region Gallery

    private static void AuditGallery(ShrineAuditSnapshot shrine, List<AuditIssueDto> issues)
    {
        if (shrine.GalleryImages.Count == 0)
        {
            AddWarning(issues, "Gallery", null, "Gallery.GalleryImages", "Shrine should have at least one gallery image.");
            return;
        }

        foreach (var image in shrine.GalleryImages)
        {
            AuditImage(image, issues, "Gallery", image.ImgId, "GalleryImage");
        }
    }

    #endregion

    #region Image

    private static bool AuditImage(
        ImageAuditSnapshot image,
        List<AuditIssueDto> issues,
        string section,
        int? itemId,
        string fieldPrefix)
    {
        var isValid = true;

        if (string.IsNullOrWhiteSpace(image.ImgSource))
        {
            AddError(issues, section, itemId, $"{fieldPrefix}.ImgSource", "Image is missing source.");
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(image.Title))
        {
            AddError(issues, section, itemId, $"{fieldPrefix}.Title", "Image is missing title.");
            isValid = false;
        }

        if (image.Citation is null)
        {
            AddError(issues, section, itemId, $"{fieldPrefix}.Citation", "Image must have a citation.");
            isValid = false;
        }
        else
        {
            if (!IsCitationValid(image.Citation, issues, section, itemId, $"{fieldPrefix}.Citation"))
                isValid = false;
        }

        return isValid;
    }

    #endregion

    #region Valid Citation

    private static bool IsCitationValid(
        CitationAuditSnapshot citation,
        List<AuditIssueDto> issues,
        string section,
        int? itemId,
        string fieldPrefix)
    {
        var isValid = true;

        if (string.IsNullOrWhiteSpace(citation.Title))
        {
            AddError(issues, section, itemId, $"{fieldPrefix}.Title", "Citation is missing title.");
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(citation.Author))
        {
            AddError(issues, section, itemId, $"{fieldPrefix}.Author", "Citation is missing author.");
            isValid = false;
        }

        if (!citation.Year.HasValue)
        {
            AddError(issues, section, itemId, $"{fieldPrefix}.Year", "Citation is missing year.");
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(citation.Url))
        {
            AddWarning(issues, section, itemId, $"{fieldPrefix}.Url", "Citation URL is suggested.");
        }
        else if (!Uri.TryCreate(citation.Url, UriKind.Absolute, out _))
        {
            AddError(issues, section, itemId, $"{fieldPrefix}.Url", "Citation URL format is invalid.");
            isValid = false;
        }

        return isValid;
    }

    #endregion

    #region Valid Email

    private static bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }

    #endregion

    #region Valid Phone

    private static bool IsValidPhone(string phone)
    {
        return Regex.IsMatch(phone, @"^[0-9+\-\(\)\s]+$");
    }

    #endregion

    #region Add Error

    private static void AddError(List<AuditIssueDto> issues, string section, int? itemId, string field, string message)
    {
        issues.Add(new AuditIssueDto
        {
            Severity = "Error",
            Section = section,
            ItemId = itemId,
            Field = field,
            Message = message
        });
    }

    #endregion

    #region Add Warning

    private static void AddWarning(List<AuditIssueDto> issues, string section, int? itemId, string field, string message)
    {
        issues.Add(new AuditIssueDto
        {
            Severity = "Warning",
            Section = section,
            ItemId = itemId,
            Field = field,
            Message = message
        });
    }

    #endregion
}
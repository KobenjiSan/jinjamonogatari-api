namespace Application.Features.Shrines.Models;

public class AuditIssueDto
{
    public string Severity { get; set; } = string.Empty; // Error / Warning
    public string Section { get; set; } = string.Empty;  // ShrineMeta / Kami / History / Folklore / etc.
    public int? ItemId { get; set; }
    public string? Field { get; set; }
    public string Message { get; set; } = string.Empty;
}
namespace Application.Features.Shrines.Models;

public class ShrineAuditDto
{
    public int ShrineId { get; set; }
    public bool IsSubmittable { get; set; }
    public int ErrorCount { get; set; }
    public int WarningCount { get; set; }
    public List<AuditIssueDto> Issues { get; set; } = new();
}
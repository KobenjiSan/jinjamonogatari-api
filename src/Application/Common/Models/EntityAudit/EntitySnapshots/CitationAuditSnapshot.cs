namespace Application.Common.Models.EntityAudit;

public class CitationAuditSnapshot
{
    public int CiteId { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Url { get; set; }
    public int? Year { get; set; }
}
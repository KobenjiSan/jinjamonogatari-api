namespace Application.Common.Models.EntityAudit;

public class KamiAuditSnapshot
{
    public int KamiId { get; set; }
    public string? NameEn { get; set; }
    public string? NameJp { get; set; }
    public string? Desc { get; set; }
    public ImageAuditSnapshot? Image { get; set; }
    public List<CitationAuditSnapshot> Citations { get; set; } = new();
}
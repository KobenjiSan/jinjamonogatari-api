namespace Application.Common.Models.EntityAudit;

public class ImageAuditSnapshot
{
    public int ImgId { get; set; }
    public string? ImageUrl { get; set; }
    public string? Title { get; set; }
    public string? Desc { get; set; }
    public CitationAuditSnapshot? Citation { get; set; }
}
using Application.Common.Models.Citations;
using Application.Features.Shrines.Models;

namespace Application.Common.Models.Images;
public record ImageCMSDto(
    int ImgId,
    string? ImageUrl,
    string? Title,
    string? Desc,
    CitationCMSDto? Citation,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record ImageCMSAuditDto(
    int ImgId,
    string? ImageUrl,
    string? Title,
    string? Desc,
    CitationCMSDto? Citation,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    EntityAuditDto? Audit = null
);
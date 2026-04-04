using Domain.Enums;

namespace Application.Features.Shrines.Models;

public record ShrineReviewDto(
    int ReviewId,
    
    // Submission Data
    DateTime SubmittedAt,
    int SubmittedBy,
    string SubmittedByUsername,

    // Review Data
    DateTime? ReviewedAt,
    int? ReviewedBy,
    string? ReviewedByUsername,

    // Comment
    string? ReviewerComment,

    // Decision
    string Decision
);


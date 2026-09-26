using Domain.Enums;

namespace Application.Features.Kami.Models;

public record KamiReviewDto(
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


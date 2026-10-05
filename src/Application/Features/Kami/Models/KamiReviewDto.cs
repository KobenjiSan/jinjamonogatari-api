using Domain.Enums;

namespace Application.Features.Kami.Models;

public record KamiReviewDto(
    int ReviewId,
    
    // Submission Data
    DateTime SubmittedAt,
    int SubmittedBy,
    string SubmittedByUsername,

    // Review Data
    DateTime? ResolvedAt,
    int? ResolvedBy,
    string? ResolvedByUsername,

    // Comment
    string? ReviewerComment,

    // Published -> Draft
    DateTime? ReturnedToDraftAt,
    int? ReturnedToDraftBy,
    string? ReturnedToDraftByUsername,

    // Decision
    string Decision
);


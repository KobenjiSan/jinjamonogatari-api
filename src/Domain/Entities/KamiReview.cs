using Domain.Enums;

namespace Domain.Entities;

public class KamiReview
{
    public int ReviewId { get; set; }
    
    public int KamiId { get; set; }
    public Kami Kami { get; set; } = null!;

    // Submission Data
    public DateTime SubmittedAt { get; set; }
    public int SubmittedBy { get; set; }
    public User SubmittedByUser { get; set; } = null!;

    // Review Data
    public DateTime? ReviewedAt { get; set; }
    public int? ReviewedBy { get; set; }
    public User? ReviewedByUser { get; set; }

    // Comment
    public string? ReviewerComment { get; set; }

    // Decision
    public ReviewDecision Decision { get; set; }
}
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

    // Resolve Data (Could be admin reviewer or editor reverting to draft)
    public DateTime? ResolvedAt { get; set; }
    public int? ResolvedBy { get; set; }
    public User? ResolvedByUser { get; set; }

    // Comment
    public string? ReviewerComment { get; set; }

    // Published -> Draft
    public DateTime? ReturnedToDraftAt { get; set; }
    public int? ReturnedToDraftBy { get; set; }
    public User? ReturnedToDraftByUser { get; set; } 

    // Decision
    public ReviewDecision Decision { get; set; }
}
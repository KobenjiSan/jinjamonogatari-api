using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class KamiReviewConfiguration : IEntityTypeConfiguration<KamiReview>
{
    public void Configure(EntityTypeBuilder<KamiReview> e)
    {
        // Map entity to table
        e.ToTable("kami_review");

        // Primary key
        e.HasKey(x => x.ReviewId);
        e.Property(x => x.ReviewId).HasColumnName("review_id").ValueGeneratedOnAdd();

        e.Property(x => x.KamiId).HasColumnName("kami_id").IsRequired();
        
        // Submission Data
        e.Property(x => x.SubmittedAt).HasColumnName("submitted_at").HasColumnType("timestamp with time zone").IsRequired();
        e.Property(x => x.SubmittedBy).HasColumnName("submitted_by").IsRequired();
        
        // Review Data
        e.Property(x => x.ReviewedAt).HasColumnName("reviewed_at").HasColumnType("timestamp with time zone");
        e.Property(x => x.ReviewedBy).HasColumnName("reviewed_by");

        e.Property(x => x.ReviewerComment).HasColumnName("reviewer_comment");

        // NOTE: HasConversion is needed as Decision is an Enum without it, it would store as an int.
        e.Property(x => x.Decision).HasColumnName("decision").HasConversion<string>().IsRequired(); 
        
        // Relationships config:
        //FK: kami_reviews.kami_id -> kami.reviews.kami_id
        e.HasOne(x => x.Kami)                       // Each KamiReview references ONE Kami
            .WithMany(k => k.Reviews)               // A Kami can have MANY KamiReview links
            .HasForeignKey(x => x.KamiId)           // FK column stored on kami_reviews
            .OnDelete(DeleteBehavior.Cascade);      // Deleting a Kami removes its links
    
        //FK: kami_review.submitted_by -> users.user_id
        e.HasOne(x => x.SubmittedByUser)
            .WithMany(r => r.SubmittedKamiReviews)
            .HasForeignKey(x => x.SubmittedBy)
            .OnDelete(DeleteBehavior.Restrict);

        //FK: kami_review.reviewed_by -> users.user_id
        e.HasOne(x => x.ReviewedByUser)
            .WithMany(r => r.ReviewedKamiReviews)
            .HasForeignKey(x => x.ReviewedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
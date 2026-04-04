using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ShrineReviewConfiguration : IEntityTypeConfiguration<ShrineReview>
{
    public void Configure(EntityTypeBuilder<ShrineReview> e)
    {
        // Map entity to table
        e.ToTable("shrine_review");

        // Primary key
        e.HasKey(x => x.ReviewId);
        e.Property(x => x.ReviewId).HasColumnName("review_id").ValueGeneratedOnAdd();

        e.Property(x => x.ShrineId).HasColumnName("shrine_id").IsRequired();
        
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
        //FK: shrine_reviews.shrine_id -> shrines.reviews.shrine_id
        e.HasOne(x => x.Shrine)                     // Each ShrineReview references ONE Shrine
            .WithMany(s => s.Reviews)               // A Shrine can have MANY ShrineReview links
            .HasForeignKey(x => x.ShrineId)         // FK column stored on shrine_reviews
            .OnDelete(DeleteBehavior.Cascade);      // Deleting a Shrine removes its links
    
        //FK: shrine_review.submitted_by -> users.user_id
        e.HasOne(x => x.SubmittedByUser)
            .WithMany(r => r.SubmittedReviews)
            .HasForeignKey(x => x.SubmittedBy)
            .OnDelete(DeleteBehavior.Restrict);

        //FK: shrine_review.reviewed_by -> users.user_id
        e.HasOne(x => x.ReviewedByUser)
            .WithMany(r => r.ReviewedReviews)
            .HasForeignKey(x => x.ReviewedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
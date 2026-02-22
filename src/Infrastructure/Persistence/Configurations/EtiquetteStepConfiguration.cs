using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Infrastructure.Persistence.Configurations;

public class EtiquetteStepConfiguration : IEntityTypeConfiguration<EtiquetteStep>
{
    public void Configure(EntityTypeBuilder<EtiquetteStep> e)
    {
        // Map entity to table
        e.ToTable("etiquette_steps");

        // Primary key
        e.HasKey(x => x.StepId);
        e.Property(x => x.StepId).HasColumnName("step_id").ValueGeneratedOnAdd();

        // Content
        e.Property(x => x.StepOrder).HasColumnName("step_order");
        e.Property(x => x.Text).HasColumnName("text");

        // Image (optional)
        e.Property(x => x.ImageId).HasColumnName("img_id");

        // Topic
        e.Property(x => x.TopicId).HasColumnName("topic_id").IsRequired();

        // Relationship config:

        e.HasOne(x => x.Image)
            .WithMany()
            .HasForeignKey(x => x.ImageId)
            .OnDelete(DeleteBehavior.SetNull);

        e.HasIndex(x => new { x.TopicId, x.StepOrder }); // for faster lookups
    }
}
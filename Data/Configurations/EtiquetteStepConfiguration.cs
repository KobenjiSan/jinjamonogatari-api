using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using API.Domain.Entities;

namespace API.Data.Configurations;

public class EtiquetteStepConfiguration : IEntityTypeConfiguration<EtiquetteStep>
{
    public void Configure(EntityTypeBuilder<EtiquetteStep> e)
    {
        e.ToTable("etiquette_steps");

        e.HasKey(x => x.StepId);
        e.Property(x => x.StepId).HasColumnName("step_id");

        e.Property(x => x.TopicId).HasColumnName("topic_id").IsRequired();
        e.Property(x => x.StepOrder).HasColumnName("step_order");
        e.Property(x => x.Text).HasColumnName("text");

        e.Property(x => x.ImageId).HasColumnName("img_id");

        e.HasOne(x => x.Image)
         .WithMany(i => i.EtiquetteSteps)
         .HasForeignKey(x => x.ImageId)
         .OnDelete(DeleteBehavior.SetNull);

        e.HasIndex(x => new { x.TopicId, x.StepOrder });
    }
}
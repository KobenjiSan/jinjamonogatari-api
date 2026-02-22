namespace Domain.Entities;

public class EtiquetteStep
{
    // PK
    public int StepId { get; set; }

    // Content
    public string? Text { get; set; }

    // Positioning
    public int? StepOrder { get; set; }

    // Image (optional)
    public int? ImageId { get; set; }
    public Image? Image { get; set; }

    // Topic
    public int TopicId { get; set; }
    public EtiquetteTopic Topic { get; set; } = null!;
}
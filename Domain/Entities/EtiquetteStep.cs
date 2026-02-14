namespace API.Domain.Entities;
public class EtiquetteStep
{
    public int StepId { get; set; }

    public int TopicId { get; set; }
    public EtiquetteTopic Topic { get; set; } = null!;

    public int? StepOrder { get; set; }
    public string? Text { get; set; }

    public int? ImageId { get; set; }
    public Image? Image { get; set; }
}
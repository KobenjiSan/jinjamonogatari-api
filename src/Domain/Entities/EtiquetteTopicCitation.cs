using Domain.Common;

namespace Domain.Entities;

public class EtiquetteTopicCitation : IHasCreatedAt
{
    public int TopicId { get; set; }
    public EtiquetteTopic Topic { get; set; } = null!;

    public int CiteId { get; set; }
    public Citation Citation { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
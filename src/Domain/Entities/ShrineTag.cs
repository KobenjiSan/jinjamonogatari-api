using Domain.Common;

namespace Domain.Entities;

public class ShrineTag : IHasCreatedAt
{
    public int ShrineId { get; set; }
    public Shrine Shrine { get; set; } = null!;

    public int TagId { get; set; }
    public Tag Tag { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
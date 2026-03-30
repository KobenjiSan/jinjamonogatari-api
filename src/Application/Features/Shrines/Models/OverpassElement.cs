namespace Application.Features.Shrines.Models;

public class OverpassElement
{
    public string Type { get; set; } = default!; // node/way/relation
    public long Id { get; set; }

    public double? Lat { get; set; }
    public double? Lon { get; set; }

    public Dictionary<string, string>? Tags { get; set; }
}

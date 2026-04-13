namespace Application.Features.Shrines.Models;

public class OverpassResponse
{
    public List<OverpassElement> Elements { get; set; } = new();
}

public class OverpassElement
{
    public string Type { get; set; } = default!; // node/way/relation
    public long Id { get; set; }

    public double? Lat { get; set; }
    public double? Lon { get; set; }
    public OverpassCenter? Center { get; set; }
    public Dictionary<string, string>? Tags { get; set; }
}

public class OverpassCenter
{
    public double Lat { get; set; }
    public double Lon { get; set; }
}
using System.Collections.Generic;

namespace LostPolygon.AspNetCore.DataSeeding;

public class DataSeedingConfiguration {
    public List<string>? DataSeeders { get; set; } = new();
}

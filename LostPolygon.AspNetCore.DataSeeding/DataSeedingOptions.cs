using System.Collections.Generic;

namespace LostPolygon.AspNetCore.DataSeeding; 

public class DataSeedingOptions {
    public List<string> DataSeeders { get; set; } = new List<string>();
}
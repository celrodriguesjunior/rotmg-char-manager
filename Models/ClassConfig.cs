using System.Collections.Generic;

namespace RotmgManager.Models;

public class ClassConfig
{
    public string ClassName { get; set; } = string.Empty;
    public Dictionary<StatType, int> MaxStats { get; set; } = new();
    public Dictionary<StatType, int> GainPerPotion { get; set; } = new();
}

using System;
using System.Collections.Generic;

namespace RotmgManager.Models;

public class Character
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public Dictionary<StatType, int> CurrentStats { get; set; } = StatDictionaryFactory.Create(0);

    public override string ToString() => Name;
}

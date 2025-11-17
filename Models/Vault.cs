using System.Collections.Generic;

namespace RotmgManager.Models;

public class Vault
{
    public Dictionary<StatType, int> Potions { get; set; } = StatDictionaryFactory.Create(0);
}

using System.Collections.Generic;

namespace RotmgManager.Models;

public class GameData
{
    public List<ClassConfig> Classes { get; set; } = new();
    public List<Character> Characters { get; set; } = new();
    public Vault Vault { get; set; } = new();
}

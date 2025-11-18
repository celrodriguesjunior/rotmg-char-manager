using System;
using System.Collections.Generic;
using System.Threading;

namespace RotmgManager.Models;

public class Character
{
    public Character() { }

    public Character(string className, Dictionary<StatType, int> maxStatus)
    {
        ClassName = className;
        CurrentStats = maxStatus;
    }

    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public Dictionary<StatType, int> CurrentStats { get; set; } = StatDictionaryFactory.Create(0);

    public int GetMaxAtribute(StatType stat) => CurrentStats[stat];

    public static int GetMaxAtributeFromClass(ClassName className, StatType stat) => CharacterMaxStatusFactory.GetCharacterDefault(className).GetMaxAtribute(stat);

    public static ClassName StrClassNameToEnum(string className) => className switch
    {
        "Rogue" => Models.ClassName.Rogue,
        "Huntress" => Models.ClassName.Huntress,
        _ => Models.ClassName.None
    };

    public override string ToString() => Name;
}

public static class CharacterMaxStatusFactory
{

    public static Character GetCharacterDefault(ClassName className)
    {
        return className switch
        {
            ClassName.Rogue => new Character(className.ToString(), GenerateDictonary(750    ,300 ,55  ,25  ,65  ,75  ,40  ,50)),
            ClassName.Archer => new Character(className.ToString(), GenerateDictonary(750   ,300 ,75  ,25  ,55  ,50  ,40  ,50)),
            ClassName.Wizard => new Character(className.ToString(), GenerateDictonary(700   ,400 ,60  ,25  ,50  ,75  ,40  ,60)),
            ClassName.Priest => new Character(className.ToString(), GenerateDictonary(700   ,400 ,65  ,25  ,55  ,60  ,40  ,75)),
            ClassName.Warrior => new Character(className.ToString(), GenerateDictonary(800  ,300 ,75  ,25  ,50  ,50  ,75  ,50)),
            ClassName.Knight => new Character(className.ToString(), GenerateDictonary(800   ,300 ,50  ,40  ,50  ,50  ,75  ,50)),
            ClassName.Paladin => new Character(className.ToString(), GenerateDictonary(800  ,300 ,55  ,30  ,55  ,55  ,60  ,75)),
            ClassName.Assassin => new Character(className.ToString(), GenerateDictonary(750 ,350 ,65  ,25  ,65  ,75  ,40  ,60)       ),
            ClassName.Necromancer => new Character(className.ToString(), GenerateDictonary(700  ,400 ,75  ,25  ,50  ,60  ,40  ,75)),
            ClassName.Huntress => new Character(className.ToString(), GenerateDictonary(750 ,350 ,65  ,25  ,50  ,60  ,40  ,60)),
            ClassName.Mystic => new Character(className.ToString(), GenerateDictonary(700   ,400 ,65  ,25  ,60  ,65  ,40  ,75)),
            ClassName.Trickster => new Character(className.ToString(), GenerateDictonary(750    ,300 ,65  ,25  ,75  ,75  ,40  ,60)),
            ClassName.Sorcerer => new Character(className.ToString(), GenerateDictonary(700 ,400 ,70  ,25  ,60  ,60  ,75  ,60)),
            ClassName.Ninja => new Character(className.ToString(), GenerateDictonary(800    ,300 ,70  ,25  ,60  ,70  ,60  ,70)),
            ClassName.Samurai => new Character(className.ToString(), GenerateDictonary(800  ,300 ,75  ,30  ,55  ,55  ,60  ,60)),
            ClassName.Bard => new Character(className.ToString(), GenerateDictonary(750 ,400 ,55  ,25  ,55  ,70  ,45  ,75)),
            ClassName.Summoner => new Character(className.ToString(), GenerateDictonary(700 ,400 ,60  ,25  ,60  ,75  ,40  ,75)),
            ClassName.Kensei => new Character(className.ToString(), GenerateDictonary(800   ,300 ,65  ,25  ,60  ,65  ,60  ,50)),
            _ => new Character(className.ToString(), GenerateDictonary(0, 0, 0, 0, 0, 0, 0, 0))
        };
    }

    private static Dictionary<StatType, int> GenerateDictonary(int life, int mana, int attack, int defense, int speed, int dex, int vit, int wis) => new()
    {
        { StatType.Life, life },
        { StatType.Mana, mana },
        { StatType.Attack, attack },
        { StatType.Defense, defense },
        { StatType.Speed, speed },
        { StatType.Dexterity, dex },
        { StatType.Vitality, vit },
        { StatType.Wisdom, wis },
     };

}

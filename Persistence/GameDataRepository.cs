using RotmgManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;

namespace RotmgManager.Persistence;

public class GameDataRepository
{
    private readonly string dataFilePath;
    private readonly JsonSerializerOptions jsonOptions;

    public GameDataRepository()
    {
        dataFilePath = Path.Combine(AppContext.BaseDirectory, "gameData.json");
        jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        jsonOptions.Converters.Add(new JsonStringEnumConverter());
    }

    public GameData Load()
    {
        if (!File.Exists(dataFilePath))
        {
            return CreateDefaultData();
        }

        try
        {
            string json = File.ReadAllText(dataFilePath);
            var data = JsonSerializer.Deserialize<GameData>(json, jsonOptions);
            if (data == null)
            {
                return CreateDefaultData();
            }

            EnsureDataIntegrity(data);
            return data;
        }
        catch
        {
            return CreateDefaultData();
        }
    }

    public void Save(GameData data)
    {
        EnsureDataIntegrity(data);
        string json = JsonSerializer.Serialize(data, jsonOptions);
        File.WriteAllText(dataFilePath, json);
    }

    private void EnsureDataIntegrity(GameData data)
    {
        data.Vault ??= new Vault();
        data.Vault.Potions ??= StatDictionaryFactory.Create(0);
        StatDictionaryFactory.EnsureAllStats(data.Vault.Potions);

        data.Classes ??= new List<ClassConfig>();
        if (data.Classes.Count == 0)
        {
            data.Classes = CreateDefaultClasses();
        }

        data.Characters ??= new List<Character>();
        foreach (Character character in data.Characters)
        {
            character.CurrentStats ??= StatDictionaryFactory.Create(0);
            StatDictionaryFactory.EnsureAllStats(character.CurrentStats);
        }
    }

    private GameData CreateDefaultData()
    {
        return new GameData
        {
            Classes = CreateDefaultClasses(),
            Characters = new List<Character>(),
            Vault = new Vault()
        };
    }

    private List<ClassConfig> CreateDefaultClasses()
    {
        return new List<ClassConfig>
        {
            CreateClassConfig(ClassName.Rogue.ToString()),
            CreateClassConfig(ClassName.Archer.ToString()),
            CreateClassConfig(ClassName.Wizard.ToString()),
            CreateClassConfig(ClassName.Priest.ToString()),
            CreateClassConfig(ClassName.Warrior.ToString()),
            CreateClassConfig(ClassName.Knight.ToString()),
            CreateClassConfig(ClassName.Paladin.ToString()),
            CreateClassConfig(ClassName.Assassin.ToString()),
            CreateClassConfig(ClassName.Necromancer.ToString()),
            CreateClassConfig(ClassName.Huntress.ToString()),
            CreateClassConfig(ClassName.Mystic.ToString()),
            CreateClassConfig(ClassName.Trickster.ToString()),
            CreateClassConfig(ClassName.Sorcerer.ToString()),
            CreateClassConfig(ClassName.Ninja.ToString()),
            CreateClassConfig(ClassName.Samurai.ToString()),
            CreateClassConfig(ClassName.Bard.ToString()),
            CreateClassConfig(ClassName.Summoner.ToString()),
            CreateClassConfig(ClassName.Kensei.ToString()),
        };
    }

    private ClassConfig CreateClassConfig(string className)
    {
        var maxStats = new Dictionary<StatType, int>
        {

            {StatType.Life, Character.GetMaxAtributeFromClass(Character.StrClassNameToEnum(className), StatType.Life) },
            {StatType.Mana, Character.GetMaxAtributeFromClass(Character.StrClassNameToEnum(className), StatType.Mana) },
            {StatType.Attack, Character.GetMaxAtributeFromClass(Character.StrClassNameToEnum(className), StatType.Attack) },
            {StatType.Defense, Character.GetMaxAtributeFromClass(Character.StrClassNameToEnum(className), StatType.Defense) },
            {StatType.Speed, Character.GetMaxAtributeFromClass(Character.StrClassNameToEnum(className), StatType.Speed) },
            {StatType.Dexterity, Character.GetMaxAtributeFromClass(Character.StrClassNameToEnum(className), StatType.Dexterity) },
            {StatType.Vitality, Character.GetMaxAtributeFromClass(Character.StrClassNameToEnum(className), StatType.Vitality) },
            {StatType.Wisdom, Character.GetMaxAtributeFromClass(Character.StrClassNameToEnum(className), StatType.Wisdom) },
        };

        var gainPerPotion = new Dictionary<StatType, int>
        {
            {StatType.Life, 5},
            {StatType.Mana, 5},
            {StatType.Attack, 1},
            {StatType.Defense, 1},
            {StatType.Speed, 1},
            {StatType.Dexterity, 1},
            {StatType.Vitality, 1},
            {StatType.Wisdom, 1}
        };

        return new ClassConfig
        {
            ClassName = className,
            MaxStats = maxStats,
            GainPerPotion = gainPerPotion
        };
    }
}



//{StatType.Life, new List<string>(){"Knight", "Warrior", "Paladin", "Ninja", "Samurai", "Kensei" }.Contains(className)  ? 800 : 
//                new List<string>(){"Rogue", "Archer", "Assassin", "Huntress", "Trickster", "Bard" }.Contains(className) ? 750 : 
//                700 },
//{StatType.Mana, new List<string>(){"Wizard", "Priest", "Necromancer", "Mystic", "Sorcerer", "Bard", "Summoner" }.Contains(className)  ? 400 : 
//                new List<string>(){"Assassin", "Huntress" }.Contains(className) ? 350 : 
//                300},
//{StatType.Attack, className is "Archer" or "Warrior" or "Necromancer" or "Samurai" ? 75 : 
//                  className is "Sorcerer" or "Ninja" ? 70 : 
//                  className is "Priest" or "Assassin" or "Huntress" or "Mystic" or "Trickster" or "Kensei" ? 65 : 
//                  className is "Wizard" or "Summoner" ? 60 : 
//                  className is "Rogue" or "Paladin" or "Bard" ? 55 : 
//                  50},
//{StatType.Defense, className is "Knight" ? 40 :
//                   className is "Paladin" or "Samurai" ? 30 :
//                   25},
//{StatType.Speed, className is "Trickster" ? 75 :
//                 className is "Rogue" or "Assassin" ? 65 :
//                 className is "Mystic" or "Sorcerer" or "Ninja" or "Summoner" or "Kensei" ? 60 :
//                 className is "Archer" or "Priest" or "Paladin" or "Samurai" or "Bard" ? 55 :
//                 50},
//{StatType.Dexterity, className is "Wizard" or "Rogue" or "Assassin" or "Trickster" or "Summoner" ? 75 :
//                     className is "Ninja" or "Bard" ? 70 :
//                     className is "Mystic" or "Kensei" ? 65 :
//                     className is "Priest" or "Assassin" or "Huntress" or "Sorcerer" ? 60 :
//                     className is "Paladin" or "Samurai" ? 55 :
//                     50},
//{StatType.Vitality, className is "Warrior" or "Knight" or "Sorcerer" ? 75 :
//                    className is "Paladin" or "Ninja" or "Samurai" or "Kensei" ? 60 :
//                    className is "Bard" ? 45 :
//                    40},
//{StatType.Wisdom, className is "Priest" or "Paladin" or "Necromancer" or "Mystic" or "Bard" or "Summoner" ? 75 :
//                  className is "Ninja" ? 70 :
//                  className is "Wizard" or "Assassin" or "Huntress" or "Trickster" or "Sorcerer" or "Samurai" ? 60 :
//                  50}
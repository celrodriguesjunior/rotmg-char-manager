using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using RotmgManager.Models;

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
            CreateClassConfig("Wizard"),
            CreateClassConfig("Knight"),
            CreateClassConfig("Priest"),
            CreateClassConfig("Assassin"),
            CreateClassConfig("Huntress"),
            CreateClassConfig("Necromancer")
        };
    }

    private ClassConfig CreateClassConfig(string className)
    {
        var maxStats = new Dictionary<StatType, int>
        {
            {StatType.Life, className == "Knight" ? 770 : 670},
            {StatType.Mana, className is "Wizard" or "Necromancer" ? 385 : 252},
            {StatType.Attack, className is "Wizard" or "Knight" ? 75 : 50},
            {StatType.Defense, className == "Knight" ? 40 : 25},
            {StatType.Speed, className == "Knight" ? 50 : 60},
            {StatType.Dexterity, className is "Wizard" or "Necromancer" ? 75 : 50},
            {StatType.Vitality, className == "Knight" ? 75 : 40},
            {StatType.Wisdom, className == "Priest" ? 75 : 60}
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

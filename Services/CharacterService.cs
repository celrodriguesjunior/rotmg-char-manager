using System;
using System.Collections.Generic;
using System.Linq;
using RotmgManager.Models;

namespace RotmgManager.Services;

public class CharacterService
{
    private readonly GameData gameData;

    public CharacterService(GameData gameData)
    {
        this.gameData = gameData;
    }

    public ClassConfig? GetClassConfig(Character character)
    {
        if (character == null)
        {
            return null;
        }

        return gameData.Classes.FirstOrDefault(cfg =>
            string.Equals(cfg.ClassName, character.ClassName, StringComparison.OrdinalIgnoreCase));
    }

    public Dictionary<StatType, int> GetMissingPots(Character character)
    {
        var result = StatDictionaryFactory.Create(0);
        var config = GetClassConfig(character);
        if (character == null || config == null)
        {
            return result;
        }

        foreach (StatType stat in Enum.GetValues(typeof(StatType)))
        {
            config.MaxStats.TryGetValue(stat, out int maxValue);
            config.GainPerPotion.TryGetValue(stat, out int gainPerPotion);
            if (gainPerPotion <= 0)
            {
                gainPerPotion = 1;
            }

            character.CurrentStats.TryGetValue(stat, out int currentValue);
            int difference = maxValue - currentValue;

            // missing potions are the ceiling of the remaining stat divided by the potion gain
            int missing = difference <= 0
                ? 0
                : (int)Math.Ceiling(difference / (double)gainPerPotion);

            result[stat] = missing;
        }

        return result;
    }
}

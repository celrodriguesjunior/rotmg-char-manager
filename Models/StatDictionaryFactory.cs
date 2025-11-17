using System;
using System.Collections.Generic;

namespace RotmgManager.Models;

public static class StatDictionaryFactory
{
    public static Dictionary<StatType, int> Create(int initialValue)
    {
        var dict = new Dictionary<StatType, int>();
        foreach (StatType stat in Enum.GetValues(typeof(StatType)))
        {
            dict[stat] = initialValue;
        }

        return dict;
    }

    public static void EnsureAllStats(Dictionary<StatType, int> stats, int defaultValue = 0)
    {
        foreach (StatType stat in Enum.GetValues(typeof(StatType)))
        {
            if (!stats.ContainsKey(stat))
            {
                stats[stat] = defaultValue;
            }
        }
    }
}

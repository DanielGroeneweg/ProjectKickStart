using UnityEngine;
using System.Collections.Generic;
public static class GACHA
{
    public static Skin RandomSkinNotUnlocked(List<Skin> skinList, Dictionary<Enums.Rarities, int> chances)
    {
        // Group skins by rarity
        List<Skin> legendaries = skinList.FindAll(s => s.rarity == Enums.Rarities.Legendary);
        List<Skin> epics = skinList.FindAll(s => s.rarity == Enums.Rarities.Epic);
        List<Skin> rares = skinList.FindAll(s => s.rarity == Enums.Rarities.Rare);
        List<Skin> commons = skinList.FindAll(s => s.rarity == Enums.Rarities.Common);

        // Roll rarity bucket
        int rng = Random.Range(0, 100);
        Enums.Rarities pickedRarity;

        if (rng < chances[Enums.Rarities.Common])
            pickedRarity = Enums.Rarities.Common;
        else if (rng < chances[Enums.Rarities.Common] + chances[Enums.Rarities.Rare])
            pickedRarity = Enums.Rarities.Rare;
        else if (rng < chances[Enums.Rarities.Common] + chances[Enums.Rarities.Rare] + chances[Enums.Rarities.Epic])
            pickedRarity = Enums.Rarities.Epic;
        else
            pickedRarity = Enums.Rarities.Legendary;

        // Try to pick from chosen bucket, else fallback
        List<Skin> bucket = pickedRarity switch
        {
            Enums.Rarities.Common => commons,
            Enums.Rarities.Rare => rares,
            Enums.Rarities.Epic => epics,
            Enums.Rarities.Legendary => legendaries,
            _ => commons
        };

        if (bucket.Count > 0)
        {
            return bucket[Random.Range(0, bucket.Count)];
        }
        else
        {
            // Fallback: pick from any non-empty bucket
            List<Skin> allAvailable = new List<Skin>();
            allAvailable.AddRange(commons);
            allAvailable.AddRange(rares);
            allAvailable.AddRange(epics);
            allAvailable.AddRange(legendaries);

            if (allAvailable.Count > 0)
            {
                return allAvailable[Random.Range(0, allAvailable.Count)];
            }

            // Nothing left at all
            return null;
        }
    }
}
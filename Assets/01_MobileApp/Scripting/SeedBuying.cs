using System.Collections.Generic;
using UnityEngine;

public class SeedBuying : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private int cost = 100;
    [SerializeField] private AvailableSkins skinList;

    [Header("Lootbox")]
    [Tooltip("Total chances should add up to 100")]
    [SerializeField] private int commonChance = 70;
    [Tooltip("Total chances should add up to 100")]
    [SerializeField] private int rareChance = 25;
    [Tooltip("Total chances should add up to 100")]
    [SerializeField] private int epicChance = 4;
    [Tooltip("Total chances should add up to 100")]
    [SerializeField] private int legendaryChance = 1;
    public void PlantSeed()
    {
        if (inventory.coins < cost) return;

        for (int i = 0; i < 20; i++)
        {
            if (inventory.seeds[i].planted) continue;

            else
            {
                inventory.seeds[i].planted = true;
                inventory.seeds[i].skin = GACHA.RandomSkinNotUnlocked(skinList.skins, new Dictionary<Enums.Rarities, int> { { Enums.Rarities.Common, commonChance }, { Enums.Rarities.Rare, rareChance }, { Enums.Rarities.Epic, epicChance }, { Enums.Rarities.Legendary, legendaryChance } });
                MoneyHandler.instance.AddCoins(-cost);
                break;
            }
        }
    }
    private void OnValidate()
    {
        int total = commonChance + rareChance + epicChance + legendaryChance;
        if (total == 100) return;
        if (total <= 0)
        {
            commonChance = 100; rareChance = epicChance = legendaryChance = 0;
            return;
        }

        float scale = 100f / total;
        float[] f = new float[4] { commonChance * scale, rareChance * scale, epicChance * scale, legendaryChance * scale };
        int[] i = new int[4] { Mathf.FloorToInt(f[0]), Mathf.FloorToInt(f[1]), Mathf.FloorToInt(f[2]), Mathf.FloorToInt(f[3]) };

        int sum = i[0] + i[1] + i[2] + i[3];
        int remainder = 100 - sum;

        // distribute remainder to the ones with largest fractional parts
        var order = new List<int> { 0, 1, 2, 3 };
        order.Sort((a, b) => (f[b] - i[b]).CompareTo(f[a] - i[a]));

        for (int k = 0; k < remainder; k++)
            i[order[k]]++;

        commonChance = i[0];
        rareChance = i[1];
        epicChance = i[2];
        legendaryChance = i[3];
    }
}
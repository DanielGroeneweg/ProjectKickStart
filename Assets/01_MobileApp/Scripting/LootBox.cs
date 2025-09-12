using UnityEngine;
using System.Collections.Generic;
public class LootBox : MonoBehaviour
{
    [SerializeField] private AvailableSkins skinList;
    [SerializeField] private Inventory inventory;
    [SerializeField] private int cost = 100;
    [Header("Skins")]
    [Tooltip("Total chances should add up to 100")]
    [SerializeField] private int commonChance = 70;
    [SerializeField] private int rareChance = 25;
    [SerializeField] private int epicChance = 4;
    [SerializeField] private int legendaryChance = 1;
    public void CheckLootBoxAvailability()
    {
        List<Skin> availableSkins = new List<Skin>();
        foreach (Skin skin in skinList.skins)
        {
            if (!inventory.unlockedSkins.Contains(skin.skinType)) availableSkins.Add(skin);
        }

        if (availableSkins.Count > 0 && inventory.coins >= cost) OpenLootBox(GACHA.RandomSkinNotUnlocked(skinList, inventory));
    }
    public void OpenLootBox(Skin skin)
    {
        inventory.coins -= cost;
        inventory.unlockedSkins.Add(skin.skinType);
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
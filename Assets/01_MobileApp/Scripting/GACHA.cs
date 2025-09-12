using UnityEngine;
using System.Collections.Generic;
public static class GACHA
{
    public static Skin RandomSkinNotUnlocked(AvailableSkins skinList, Inventory inventory)
    {
        List<Skin> availableSkins = new List<Skin>();

        foreach(Skin skin in skinList.skins)
        {
            if (!inventory.unlockedSkins.Contains(skin.skinType)) availableSkins.Add(skin);
        }

        if (availableSkins.Count > 0)
        {
            return availableSkins[Random.Range(0, availableSkins.Count)];
        }

        else
        {
            return null;
        }
    }
}
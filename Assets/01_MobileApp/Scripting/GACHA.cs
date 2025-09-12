using UnityEngine;
using System.Collections.Generic;
public class GACHA : MonoBehaviour
{
    [SerializeField] private List<Skin> skinList = new List<Skin>();
    public Skin RandomSkinNotUnlocked(Inventory inventory)
    {
        List<Skin> availableSkins = new List<Skin>();

        foreach(Skin skin in skinList)
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
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.UI;
public class GrowAPlant : MonoBehaviour
{
    [SerializeField] private float distanceToGrow = 30;
    [Serializable]
    public class Seed
    {
        public bool planted = false;
        public float distance = 0;
        public Skin skin;
    }
    [SerializeField] private Inventory inventory;
    [SerializeField] private RawImage[] seeds = new RawImage[20];
    [SerializeField] private Texture firstPhase;
    [SerializeField] private Texture secondPhase;
    [SerializeField] private Texture thirdPhase;
    [SerializeField] private Texture commonFourthPhase;
    [SerializeField] private Texture rareFourthPhase;
    [SerializeField] private Texture epicFourthPhase;
    [SerializeField] private Texture legendaryFourthPhase;
    [SerializeField] private Texture noSeed;
    [SerializeField] private UnityEvent<Skin> Unlocked;
    private void OnEnable()
    {
        UpdateSeeds();
    }
    public void UpdateSeeds()
    {
        for (int i = 0; i < seeds.Length; i++)
        {
            if (inventory.seeds[i].planted)
            {
                seeds[i].texture = noSeed;
                Color color = seeds[i].color;
                color.a = 255;
                seeds[i].color = color;

                float amount = inventory.seeds[i].distance / distanceToGrow;

                if (amount < 0.25f) seeds[i].texture = firstPhase;
                else if (amount < 0.5f) seeds[i].texture = secondPhase;
                else if (amount < 0.75f) seeds[i].texture = thirdPhase;

                else
                {
                    if (inventory.seeds[i].skin.rarity == Enums.Rarities.Common) seeds[i].texture = commonFourthPhase;
                    if (inventory.seeds[i].skin.rarity == Enums.Rarities.Rare) seeds[i].texture = rareFourthPhase;
                    if (inventory.seeds[i].skin.rarity == Enums.Rarities.Epic) seeds[i].texture = epicFourthPhase;
                    if (inventory.seeds[i].skin.rarity == Enums.Rarities.Legendary) seeds[i].texture = legendaryFourthPhase;
                }
            }
            else
            {
                seeds[i].texture = noSeed;
                Color color = seeds[i].color;
                color.a = 0;
                seeds[i].color = color;
            }
        }
    }
    public void Unlock(int i)
    {
        float amount = inventory.seeds[i].distance / distanceToGrow;

        Debug.Log(amount);

        if (amount >= 1)
        {
            inventory.unlockedSkins.Add(inventory.seeds[i].skin);
            Unlocked?.Invoke(inventory.seeds[i].skin);
            seeds[i].texture = noSeed;
            inventory.seeds[i].planted = false;
            inventory.seeds[i].distance = 0;
        }
    }
}
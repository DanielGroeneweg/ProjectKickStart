using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
public class GrowAPlant : MonoBehaviour
{
    [SerializeField] private float distanceToGrow = 30;
    [SerializeField] private LootBox lootbox;
    [Serializable]
    public class Seed
    {
        public bool planted = false;
        public float distance = 0;
    }
    [SerializeField] private Inventory inventory;
    [SerializeField] private RawImage[] seeds = new RawImage[20];
    [SerializeField] private Texture firstPhase;
    [SerializeField] private Texture secondPhase;
    [SerializeField] private Texture thirdPhase;
    [SerializeField] private Texture fourthPhase;
    [SerializeField] private Texture noSeed;
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
                float amount = inventory.seeds[i].distance / distanceToGrow;

                if (amount < 0.25f) seeds[i].texture = firstPhase;
                else if (amount < 0.5f) seeds[i].texture = secondPhase;
                else if (amount < 0.75f) seeds[i].texture = thirdPhase;
                else if (amount < 1) seeds[i].texture = fourthPhase;

                if (amount >= 1)
                {
                    lootbox.Open();
                    seeds[i].texture = noSeed;
                    inventory.seeds[i].planted = false;
                    inventory.seeds[i].distance = 0;
                }
            }
            else seeds[i].texture = noSeed;
        }
    }
}
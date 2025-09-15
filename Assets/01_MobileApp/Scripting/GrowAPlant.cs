using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
public class GrowAPlant : MonoBehaviour
{
    [SerializeField] private float distanceToGrow = 30;
    [SerializeField] private LootBox lootbox;
    [SerializeField] private int cost = 100;
    [Serializable]
    public class Seed
    {
        public bool planted = false;
        public float distance = 0;
    }
    [SerializeField] private Inventory inventory;
    [SerializeField] private RawImage[] seeds = new RawImage[20];
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
                Color color = new Color(amount, amount, amount);
                seeds[i].color = color;

                if (amount >= 1)
                {
                    lootbox.Open();
                    seeds[i].color = Color.red;
                    inventory.seeds[i].planted = false;
                    inventory.seeds[i].distance = 0;
                }
            }
            else seeds[i].color = Color.red;
        }
    }
    public void PlantSeed()
    {
        if (inventory.coins < cost) return;

        for (int i = 0; i < seeds.Length; i++)
        {
            if (inventory.seeds[i].planted) continue;

            else
            {
                inventory.seeds[i].planted = true;
                MoneyHandler.instance.AddCoins(-cost);
                UpdateSeeds();
                break;
            }
        }
    }
}
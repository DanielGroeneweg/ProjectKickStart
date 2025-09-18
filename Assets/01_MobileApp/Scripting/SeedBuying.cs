using UnityEngine;

public class SeedBuying : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private int cost = 100;
    public void PlantSeed()
    {
        if (inventory.coins < cost) return;

        for (int i = 0; i < 20; i++)
        {
            if (inventory.seeds[i].planted) continue;

            else
            {
                inventory.seeds[i].planted = true;
                MoneyHandler.instance.AddCoins(-cost);
                //UpdateSeeds();
                break;
            }
        }
    }
}
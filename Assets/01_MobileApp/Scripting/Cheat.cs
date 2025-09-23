using UnityEngine;
public class Cheat : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    public void CheatInventory()
    {
        foreach (GrowAPlant.Seed seed in inventory.seeds)
        {
            if (seed.planted)
            {
                seed.distance += 2;
            }
        }
    }
}
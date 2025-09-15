using UnityEngine;
using UnityEngine.Events;
public class SkinBuyer : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private UnityEvent<int> skinBought;
    public void BuySkin(Skin skin)
    {
        if (skin.cost <= inventory.coins && !inventory.unlockedSkins.Contains(skin))
        {
            inventory.unlockedSkins.Add(skin);
            skinBought.Invoke(-skin.cost);
        }
    }
}
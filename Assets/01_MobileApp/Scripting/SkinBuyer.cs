using UnityEngine;
using UnityEngine.Events;
public class SkinBuyer : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private UnityEvent<int> skinBought;
    public void BuySkin(Skin skin)
    {
        if (skin.cost <= inventory.coins && !inventory.unlockedSkins.Contains(skin.skinType))
        {
            inventory.unlockedSkins.Add(skin.skinType);
            skinBought.Invoke(-skin.cost);
        }
    }
}
using UnityEngine;
using TMPro;
public class SkinTextUpdater : MonoBehaviour
{
    [SerializeField] Skin skin;
    [SerializeField] TMP_Text text;
    [SerializeField] Inventory inventory;
    private void Start()
    {
        UpdateText();
    }
    public void UpdateText()
    {
        if (inventory.unlockedSkins.Contains(skin))
        text.text = $"{skin.name} \n {skin.rarity} \n unlocked";

        else text.text = $"{skin.name} \n {skin.rarity} \n locked";
    }
}

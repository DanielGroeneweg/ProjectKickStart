using UnityEngine;
using UnityEngine.UI;
public class SkinEquip : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    public Skin skin;
    public RawImage image;
    public void Equip()
    {
        if (skin == null) return;

        switch(skin.skinType)
        {
            case Enums.SkinTypes.Flower:
                inventory.equippedSkin.flower = skin;
                break;
            case Enums.SkinTypes.Stem:
                inventory.equippedSkin.stem = skin;
                break;
            case Enums.SkinTypes.Pot:
                inventory.equippedSkin.pot = skin;
                break;
            case Enums.SkinTypes.Hat:
                inventory.equippedSkin.hat = skin;
                break;
            case Enums.SkinTypes.Glasses:
                inventory.equippedSkin.glasses = skin;
                break;
            case Enums.SkinTypes.Scarf:
                inventory.equippedSkin.scarf = skin;
                break;
        }

        OverLayPlant.instance.SetSkin(skin, skin.skinType);
    }
}
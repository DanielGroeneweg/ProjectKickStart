using UnityEngine;
using UnityEngine.UI;
public class OverLayPlant : MonoBehaviour
{
    [SerializeField] private SpriteRenderer happyFace;
    [SerializeField] private SpriteRenderer sadFace;
    [SerializeField] private SpriteRenderer flower;
    [SerializeField] private SpriteRenderer stem;
    [SerializeField] private SpriteRenderer pot;
    [SerializeField] private SpriteRenderer scarf;
    [SerializeField] private SpriteRenderer hat;
    [SerializeField] private SpriteRenderer glasses;
    [SerializeField] private Inventory inventory;
    public void SetEmotion(Enums.States plantState)
    {
        bool isHappy = plantState == Enums.States.Happy;
        happyFace.gameObject.SetActive(isHappy);
        sadFace.gameObject.SetActive(!isHappy);

        SetSkin(inventory.equippedSkin.flower);
        SetSkin(inventory.equippedSkin.hat);
        SetSkin(inventory.equippedSkin.scarf);
        SetSkin(inventory.equippedSkin.glasses);
        SetSkin(inventory.equippedSkin.stem);
        SetSkin(inventory.equippedSkin.pot);
    }
    public void SetSkin(Skin skin)
    {
        switch(skin.skinType)
        {
            case Enums.SkinTypes.Hat:
                if (skin != null) hat.sprite = skin.skin;
                else hat.sprite = null;
                break;
            case Enums.SkinTypes.Glasses:
                if (skin != null) glasses.sprite = skin.skin;
                else glasses.sprite = null;
                break;
            case Enums.SkinTypes.Scarf:
                if (skin != null) scarf.sprite = skin.skin;
                else scarf.sprite = null;
                break;
            case Enums.SkinTypes.Stem:
                stem.sprite = skin.skin;
                break;
            case Enums.SkinTypes.Pot:
                pot.sprite = skin.skin;
                break;
            case Enums.SkinTypes.Flower:
                flower.sprite = skin.skin;
                break;
        }
    }
}
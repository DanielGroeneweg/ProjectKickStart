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

    public static OverLayPlant instance;
    private void OnEnable()
    {
        instance = this;

        SetSkin(inventory.equippedSkin.flower);
        SetSkin(inventory.equippedSkin.hat, Enums.SkinTypes.Hat);
        SetSkin(inventory.equippedSkin.scarf, Enums.SkinTypes.Scarf);
        SetSkin(inventory.equippedSkin.glasses, Enums.SkinTypes.Glasses);
        SetSkin(inventory.equippedSkin.stem);
        SetSkin(inventory.equippedSkin.pot);
    }
    public void SetEmotion(Enums.States plantState)
    {
        bool isHappy = plantState == Enums.States.Happy;
        happyFace.gameObject.SetActive(isHappy);
        sadFace.gameObject.SetActive(!isHappy);

        SetSkin(inventory.equippedSkin.flower);
        SetSkin(inventory.equippedSkin.hat, Enums.SkinTypes.Hat);
        SetSkin(inventory.equippedSkin.scarf, Enums.SkinTypes.Scarf);
        SetSkin(inventory.equippedSkin.glasses, Enums.SkinTypes.Glasses);
        SetSkin(inventory.equippedSkin.stem);
        SetSkin(inventory.equippedSkin.pot);
    }
    public void SetSkin(Skin skin, Enums.SkinTypes type = Enums.SkinTypes.Flower)
    {
        if (skin == null)
        {
            switch (type)
            {
                case Enums.SkinTypes.Glasses:
                    glasses.gameObject.SetActive(false);
                    break;
                case Enums.SkinTypes.Hat:
                    hat.gameObject.SetActive(false);
                    break;
                case Enums.SkinTypes.Scarf:
                    scarf.gameObject.SetActive(false);
                    break;
            }
        }

        else
        {
            switch (skin.skinType)
            {
                case Enums.SkinTypes.Stem:
                    stem.sprite = skin.skin;
                    break;
                case Enums.SkinTypes.Pot:
                    pot.sprite = skin.skin;
                    break;
                case Enums.SkinTypes.Flower:
                    flower.sprite = skin.skin;
                    break;
                case Enums.SkinTypes.Glasses:
                    glasses.gameObject.SetActive(true);
                    glasses.sprite = skin.skin;
                    break;
                case Enums.SkinTypes.Hat:
                    hat.gameObject.SetActive(true);
                    hat.sprite = skin.skin;
                    break;
                case Enums.SkinTypes.Scarf:
                    scarf.gameObject.SetActive(true);
                    scarf.sprite = skin.skin;
                    break;
            }
        }
    }
}
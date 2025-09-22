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

        SetSkin(inventory.equippedSkin);
    }
    public void SetSkin(Skin skin)
    {
        switch(skin.skinType)
        {
            case Enums.SkinTypes.Hat:
                hat.sprite = skin.skin;
                break;
            case Enums.SkinTypes.Glasses:
                glasses.sprite = skin.skin;
                break;
            case Enums.SkinTypes.Scarf:
                scarf.sprite = skin.skin;
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
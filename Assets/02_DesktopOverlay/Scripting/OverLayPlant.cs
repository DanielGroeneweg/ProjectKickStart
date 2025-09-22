using UnityEngine;
using UnityEngine.UI;
public class OverLayPlant : MonoBehaviour
{
    [SerializeField] private SpriteRenderer happyFace;
    [SerializeField] private SpriteRenderer sadFace;
    [SerializeField] private SpriteRenderer flower;
    [SerializeField] private Inventory inventory;
    public void SetEmotion(Enums.States plantState)
    {
        Debug.Log(plantState);
        bool isHappy = plantState == Enums.States.Happy;
        happyFace.gameObject.SetActive(isHappy);
        sadFace.gameObject.SetActive(!isHappy);

        SetSkin(inventory.equippedSkin);
    }
    public void SetSkin(Skin skin)
    {
        happyFace.sprite = skin.happyFace;
        sadFace.sprite = skin.sadFace;
        flower.sprite = skin.flower;
    }
}
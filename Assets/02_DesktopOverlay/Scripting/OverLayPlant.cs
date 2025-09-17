using UnityEngine;
using UnityEngine.UI;
public class OverLayPlant : MonoBehaviour
{
    [SerializeField] private RawImage happyFace;
    [SerializeField] private RawImage sadFace;
    [SerializeField] private RawImage flower;
    [SerializeField] private Inventory inventory;
    public void SetEmotion(Enums.States plantState)
    {
        bool isHappy = plantState == Enums.States.Happy;
        happyFace.gameObject.SetActive(isHappy);
        sadFace.gameObject.SetActive(isHappy);

        happyFace.texture = inventory.equippedSkin.happyFace;
        sadFace.texture = inventory.equippedSkin.sadFace;
        flower.texture = inventory.equippedSkin.flower;
    }
    public void SetSkin(Skin skin)
    {
        happyFace.texture = skin.happyFace;
        sadFace.texture = skin.sadFace;
        flower.texture = skin.flower;
    }
}
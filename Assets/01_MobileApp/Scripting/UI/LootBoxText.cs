using UnityEngine;
using TMPro;
public class LootBoxText : MonoBehaviour
{
    [SerializeField] private TMP_Text display;
    public void UpdateText(Skin skin)
    {
        display.text = $"Unlocked: \n {skin.name}";
    }
}
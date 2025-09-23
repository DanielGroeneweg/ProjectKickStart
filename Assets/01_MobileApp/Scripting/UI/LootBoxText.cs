using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;

public class LootBoxText : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text display;
    [SerializeField] private RawImage plant;
    [SerializeField] private RawImage item;

    [Header("images")]
    [SerializeField] private Texture commonPlant;
    [SerializeField] private Texture rarePlant;
    [SerializeField] private Texture epicPlant;
    [SerializeField] private Texture legendaryPlant;
    public void SetDisplay(Skin skin)
    {
        display.text = $"{skin.rarity}";
        plant.texture = skin.rarity switch
        {
            Enums.Rarities.Common => commonPlant,
            Enums.Rarities.Rare => rarePlant,
            Enums.Rarities.Epic => epicPlant,
            Enums.Rarities.Legendary => legendaryPlant,
            _ => commonPlant
        };

        // assume "sprite" is your Sprite object
        Sprite sprite = skin.skin;
        var croppedTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);

        var pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                (int)sprite.textureRect.y,
                                                (int)sprite.textureRect.width,
                                                (int)sprite.textureRect.height);

        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();
        item.texture = croppedTexture;
    }
}
using UnityEngine;
using System.Collections.Generic;
public class DressupTab : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private SkinEquip prefab;
    private List<SkinEquip> skins = new List<SkinEquip>();
    private void OnEnable()
    {
        for (int i = skins.Count - 1; i >= 0; i--)
        {
            SkinEquip skin = skins[i];
            skins.Remove(skin);
            Destroy(skin.gameObject);
        }

        foreach (Skin skin in inventory.unlockedSkins)
        {
            bool isInList = false;

            foreach (SkinEquip skinInList in skins)
            {
                if (skinInList.skin == skin) isInList = true;
            }

            if (isInList) continue;

            SkinEquip obj = Instantiate(prefab, transform);
            obj.skin = skin;

            Sprite sprite = skin.skin;
            var croppedTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);

            var pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                    (int)sprite.textureRect.y,
                                                    (int)sprite.textureRect.width,
                                                    (int)sprite.textureRect.height);

            croppedTexture.SetPixels(pixels);
            croppedTexture.Apply();
            obj.image.texture = croppedTexture;
            skins.Add(obj);
        }
    }
}

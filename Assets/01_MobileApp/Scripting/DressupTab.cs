using UnityEngine;
public class DressupTab : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private SkinEquip prefab;
    private void OnEnable()
    {
        foreach (Skin skin in inventory.unlockedSkins)
        {
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
        }
    }
}

using UnityEngine;
using System.Collections.Generic;
using System.IO;
public class DebuggingReset : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private Skin defaultFlower;
    [SerializeField] private Skin defaultStem;
    [SerializeField] private Skin defaultPot;
    public void ResetInventory()
    {
        inventory.unlockedSkins = new List<Skin> { defaultFlower, defaultStem, defaultPot };
        inventory.coins = 0;
        inventory.distanceWalked = 0;
        inventory.seeds = new GrowAPlant.Seed[20];
        inventory.username = "daniel";
        inventory.equippedSkin.flower = defaultFlower;
        inventory.equippedSkin.stem = defaultStem;
        inventory.equippedSkin.pot = defaultPot;
        inventory.hasWalked = false;

        SaveData data = new SaveData
        {
            coins = inventory.coins,
            unlockedSkinsIDs = new List<string> { defaultFlower.ID, defaultPot.ID, defaultStem.ID },
            distance = inventory.distanceWalked,
            seeds = inventory.seeds,
            username = inventory.username,
            equippedFlowerID = inventory.equippedSkin.flower.ID,
            equippedStemID = inventory.equippedSkin.stem.ID,
            equippedPotID = inventory.equippedSkin.pot.ID,
        };

        data.equippedScarfID = string.Empty;
        data.equippedHatID = string.Empty;
        data.equippedGlassesID = string.Empty;

        if (inventory.equippedSkin.scarf != null) data.equippedScarfID = inventory.equippedSkin.scarf.ID;
        if (inventory.equippedSkin.hat != null) data.equippedHatID = inventory.equippedSkin.hat.ID;
        if (inventory.equippedSkin.glasses != null) data.equippedGlassesID = inventory.equippedSkin.glasses.ID;

        OverLayPlant.instance.SetSkin(inventory.equippedSkin.flower);
        OverLayPlant.instance.SetSkin(inventory.equippedSkin.pot);
        OverLayPlant.instance.SetSkin(inventory.equippedSkin.stem);
        OverLayPlant.instance.SetSkin(inventory.equippedSkin.hat, Enums.SkinTypes.Hat);
        OverLayPlant.instance.SetSkin(inventory.equippedSkin.scarf, Enums.SkinTypes.Scarf);
        OverLayPlant.instance.SetSkin(inventory.equippedSkin.glasses, Enums.SkinTypes.Glasses);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.persistentDataPath + "/inventory.json", json);
    }
}
using UnityEngine;
using System.Collections.Generic;
using System.IO;
public class DebuggingReset : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private Skin defaultSkin;
    [SerializeField] private MoneyDisplay moneyDisplay;
    public void ResetInventory()
    {
        inventory.unlockedSkins = new List<Skin> { defaultSkin };
        inventory.coins = 0;
        inventory.distanceWalked = 0;
        inventory.seeds = new GrowAPlant.Seed[20];

        SaveData data = new SaveData
        {
            coins = inventory.coins,
            unlockedSkinsIDs = new List<string> { defaultSkin.ID },
            distance = inventory.distanceWalked,
            seeds = new GrowAPlant.Seed[20]
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.persistentDataPath + "/inventory.json", json);

        moneyDisplay.UpdateMoney();
    }
}
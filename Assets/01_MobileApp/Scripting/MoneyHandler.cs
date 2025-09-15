using UnityEngine;
using System.IO;
using System.Collections.Generic;
public class MoneyHandler : MonoBehaviour
{
    public Inventory inventory;
    public static MoneyHandler instance;

    private static string savePath;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            savePath = Application.persistentDataPath + "/inventory.json";
        }
        else Destroy(gameObject);
    }
    public void AddCoins(int amount)
    {
        inventory.coins += amount;

        List<string> ids = new List<string>();
        foreach(Skin skin in inventory.unlockedSkins)
        {
            ids.Add(skin.ID);
        }

        // wrap in class
        SaveData data = new SaveData {
            coins = inventory.coins,
            unlockedSkinsIDs = ids,
            distance = inventory.distanceWalked
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }
}
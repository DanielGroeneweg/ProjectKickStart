using UnityEngine;
using System.IO;
public class MoneyHandler : MonoBehaviour
{
    public Inventory inventory;

    private static string savePath;
    private void Awake()
    {
        savePath = Application.persistentDataPath + "/inventory.json";
    }
    public void AddCoins(int amount)
    {
        inventory.coins += amount;

        // wrap in class
        SaveData data = new SaveData {
            coins = inventory.coins,
            unlockedSkins = inventory.unlockedSkins
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }
}
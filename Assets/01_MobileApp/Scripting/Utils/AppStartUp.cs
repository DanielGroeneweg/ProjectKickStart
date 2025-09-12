using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class AppStartUp : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    private static string savePath;
    private void Awake()
    {
        savePath = Application.persistentDataPath + "/inventory.json";
    }
    private void Start()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            inventory.coins = data.coins;
            inventory.unlockedSkins = data.unlockedSkins;
        }
        else
        {
            inventory.coins = 0;
            inventory.unlockedSkins = new List<Enums.SkinTypes> { Enums.SkinTypes.Default };
        }
    }
}
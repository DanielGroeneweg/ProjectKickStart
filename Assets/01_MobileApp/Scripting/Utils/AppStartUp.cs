using System.Collections.Generic;
using UnityEngine;
using System.IO;
/// <summary>
/// Loads data needed (like the inventory object)
/// </summary>
public class AppStartUp : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private Skin defaultSkin;
    [SerializeField] private AvailableSkins availableSkins;
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
            
            foreach (string id in data.unlockedSkinsIDs)
            {
                inventory.unlockedSkins.Add(availableSkins.skins.Find(s => s.ID == id));
            }

            inventory.distanceWalked = data.distance;
        }
        else
        {
            inventory.coins = 0;
            inventory.unlockedSkins = new List<Skin> { defaultSkin };
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Events;
/// <summary>
/// Loads data needed (like the inventory object)
/// </summary>
public class AppStartUp : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private Skin defaultSkin;
    [SerializeField] private AvailableSkins availableSkins;
    [SerializeField] private UnityEvent NoSaveFile;
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
            
            inventory.unlockedSkins = new List<Skin>();
            foreach (string id in data.unlockedSkinsIDs)
            {
                inventory.unlockedSkins.Add(availableSkins.skins.Find(s => s.ID == id));
            }

            inventory.seeds = data.seeds ?? new GrowAPlant.Seed[20];
            inventory.distanceWalked = data.distance;

            inventory.equippedSkin = availableSkins.skins.Find(s => s.ID == data.equippedSkinID);
            inventory.username = data.username;
        }
        else
        {
            inventory.coins = 0;
            inventory.unlockedSkins = new List<Skin> { defaultSkin };
            inventory.seeds = new GrowAPlant.Seed[20];
            inventory.distanceWalked = 0;
            inventory.equippedSkin = defaultSkin;
            inventory.username = string.Empty;
            inventory.hasWalked = true;
            NoSaveFile?.Invoke();
        }
    }
}
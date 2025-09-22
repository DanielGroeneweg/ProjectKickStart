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
    [SerializeField] private Skin defaultFlower;
    [SerializeField] private Skin defaultPot;
    [SerializeField] private Skin defaultStem;
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

            inventory.equippedSkin.flower = availableSkins.skins.Find(s => s.ID == data.equippedFlowerID);
            inventory.equippedSkin.stem = availableSkins.skins.Find(s => s.ID == data.equippedStemID);
            inventory.equippedSkin.pot = availableSkins.skins.Find(s => s.ID == data.equippedPotID);

            inventory.equippedSkin.hat = null;
            inventory.equippedSkin.glasses = null;
            inventory.equippedSkin.scarf = null;

            if (!string.IsNullOrEmpty(data.equippedHatID)) inventory.equippedSkin.hat = availableSkins.skins.Find(s => s.ID == data.equippedHatID);
            if (!string.IsNullOrEmpty(data.equippedScarfID)) inventory.equippedSkin.scarf = availableSkins.skins.Find(s => s.ID == data.equippedScarfID);
            if (!string.IsNullOrEmpty(data.equippedGlassesID)) inventory.equippedSkin.glasses = availableSkins.skins.Find(s => s.ID == data.equippedGlassesID);
            inventory.username = data.username;
        }
        else
        {
            inventory.coins = 0;
            inventory.unlockedSkins = new List<Skin> { defaultFlower, defaultPot, defaultStem };
            inventory.seeds = new GrowAPlant.Seed[20];
            inventory.distanceWalked = 0;
            inventory.equippedSkin = new FlowerSkin
            {
                flower = defaultFlower,
                stem = defaultStem,
                pot = defaultPot
            };
            inventory.username = string.Empty;
            inventory.hasWalked = true;
            NoSaveFile?.Invoke();
        }
    }
}
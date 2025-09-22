using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class SaveData
{
    public int coins;
    public List<string> unlockedSkinsIDs;
    public float distance;
    public GrowAPlant.Seed[] seeds;
    public string equippedFlowerID;
    public string equippedStemID;
    public string equippedPotID;
    public string equippedHatID;
    public string equippedGlassesID;
    public string equippedScarfID;
    public string username;
}
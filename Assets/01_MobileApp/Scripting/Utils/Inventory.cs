using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable Objects/Inventory")]
public class Inventory : ScriptableObject
{
    public int coins = 0;
    public List<Skin> unlockedSkins = new();
    public float distanceWalked = 0;
    public GrowAPlant.Seed[] seeds = new GrowAPlant.Seed[20];
}
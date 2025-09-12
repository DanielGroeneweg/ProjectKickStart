using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable Objects/Inventory")]
public class Inventory : ScriptableObject
{
    public int coins = 0;
    public List<Enums.SkinTypes> unlockedSkins = new();
}
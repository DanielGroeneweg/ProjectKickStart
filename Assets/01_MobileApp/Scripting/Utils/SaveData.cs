using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class SaveData : MonoBehaviour
{
    public int coins;
    public List<Enums.SkinTypes> unlockedSkins = new();
}
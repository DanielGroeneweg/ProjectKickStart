using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "AvailableSkins", menuName = "Scriptable Objects/AvailableSkins")]
public class AvailableSkins : ScriptableObject
{
    public List<Skin> skins;
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
[CreateAssetMenu(fileName = "Skin", menuName = "Scriptable Objects/Skin")]
public class Skin : ScriptableObject
{
    public Texture happyFace;
    public Texture sadFace;
    public Texture flower;
    [Tooltip("The rarity of the skin")]
    public Enums.Rarities rarity;

    [SerializeField, HideInInspector] private string _ID;
    public string ID => _ID;
#if UNITY_EDITOR
    // Runs in editor when ScriptableObject is created or validated
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(_ID))
        {
            _ID = Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this); // marks asset as changed
        }
    }
#endif
}
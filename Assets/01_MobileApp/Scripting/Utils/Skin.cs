using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
[CreateAssetMenu(fileName = "Skin", menuName = "Scriptable Objects/Skin")]
public class Skin : ScriptableObject
{
    [Serializable] public class StateSkins
    {
        public RawImage skin;
        public Enums.States state;
    }
    [Tooltip("The rarity of the skin")]
    public Enums.Rarities rarity;
    [Tooltip("All emotion versions of this skin, if an emotion is not set up it well default back to default happy skin")]
    public List<StateSkins> stateSkins = new List<StateSkins>();

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
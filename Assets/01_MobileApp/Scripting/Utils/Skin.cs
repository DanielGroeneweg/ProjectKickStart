using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
[CreateAssetMenu(fileName = "Skin", menuName = "Scriptable Objects/Skin")]
public class Skin : ScriptableObject
{
    public Enums.SkinTypes skinType;
    [Serializable] public class StateSkins
    {
        public RawImage skin;
        public Enums.States state;
    }
    [Tooltip("The cost to unlock this skin")]
    public int cost = 10;
    [Tooltip("All emotion versions of this skin, if an emotion is not set up it well default back to default happy skin")]
    public List<StateSkins> stateSkins = new List<StateSkins>();
}
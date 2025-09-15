#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Skin), true)]
public class SkinInspectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Skin skin = (Skin)target;
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Skin ID", skin.ID);
    }
}
#endif
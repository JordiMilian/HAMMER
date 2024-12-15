using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerStats))]
public class PlayerStatsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlayerStats reference = (PlayerStats)target;
        if(reference.baseStats != null)
        {
            GUILayout.Space(10);
            if(GUILayout.Button("Reset Level 1"))
            {
                reference.CopyData(reference.baseStats);
            }
        }
    }
}

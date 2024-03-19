using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Locations))]
public class LocationsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Locations locations = (Locations)target;

        if (GUILayout.Button("Repopulate Locations"))
        {
            locations.Repopulate();
        }
    }
}

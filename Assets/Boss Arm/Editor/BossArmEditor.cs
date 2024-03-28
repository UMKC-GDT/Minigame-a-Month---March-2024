using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BossArmController))]
public class BossArmEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BossArmController bossArmController = (BossArmController)target;

        if(GUILayout.Button("Grab Employee"))
        {
            bossArmController.StartChaseUnderling();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Stage))]
public class StageEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var stage = target as Stage;

        if(GUILayout.Button("타일 생성"))
        {
            stage.CreateGrid();
        }

        if(GUILayout.Button("타일 제거"))
        {
            stage.DestroyGrid();
        }
    }
}

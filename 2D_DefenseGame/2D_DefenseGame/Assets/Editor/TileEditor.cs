using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor;

[CustomEditor(typeof(Tile)), CanEditMultipleObjects]
public class TileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var tiles = targets.Cast<Tile>().ToArray();
        bool bIsValid = false;

        if(GUILayout.Button("컴포넌트 모두 제거"))
        {
            bIsValid = true;
            foreach(var tile in tiles)
            {
                tile.RemoveAllTileComponents();
            }
        }

        if(GUILayout.Button("평지 추가"))
        {
            bIsValid = true;
            foreach(var tile in tiles)
            {
                var route = Instantiate(GamePrefabManager.Instance.routePrefab);
                var ground = Instantiate(GamePrefabManager.Instance.groundPrefab);

                tile.RemoveTileComponent(route);
                tile.AddTileComponentEditor(ground);
            }
        }

        if(GUILayout.Button("경로 추가"))
        {
            bIsValid = true;
            foreach(var tile in tiles)
            {
                var ground = Instantiate(GamePrefabManager.Instance.groundPrefab);
                var route = Instantiate(GamePrefabManager.Instance.routePrefab);

                tile.RemoveTileComponent(ground);
                tile.AddTileComponentEditor(route);
            }
        }

        if (GUILayout.Button("타워 추가"))
        {
            bIsValid = true;
            foreach(var tile in tiles)
            {
                var tower = Instantiate(GamePrefabManager.Instance.towerPrefab);
                tile.AddTileComponentEditor(tower);
            }
        }

        if (bIsValid)
        {
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if(prefabStage != null)
            {
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            }
        }
    }
}

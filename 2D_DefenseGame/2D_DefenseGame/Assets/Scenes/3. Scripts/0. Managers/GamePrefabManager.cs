using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SingletonPattern;

[CreateAssetMenu(fileName = "Game Prefab Manager", menuName = "Singleton/Game Prefab Manager")]
public class GamePrefabManager : SingletonScriptableObject<GamePrefabManager>
{
    // Ground
    public Ground groundPrefab;
    
    // Route
    public Route routePrefab;

    // Tower
    public Tower towerPrefab;

    // Start
    public StartPoint startPointPrefab;

    // End
    public EndPoint endPointPrefab;
}

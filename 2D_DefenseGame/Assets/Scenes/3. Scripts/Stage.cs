using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField]
    private Vector2Int Size;

    public Tile[,] tiles;
    private Vector2Int[] Waypoints { get; set; }
    public Wave wave;
    
    // 보류
    private enum StageState
    {
        Ongoing, Failed, Success
    }
    private StageState stageState { get; set; }
    
    #region Grid
    public void CreateGrid()
    {
        tiles = new Tile[Size.x, Size.y];
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                var tileGameObject = new GameObject($"Tile ({x}, {y})");
                var tile = tileGameObject.AddComponent<Tile>();

                tiles[x, y] = tile;
                // 타일 초기화
                tile.transform.parent = transform;
                tile.transform.localPosition = new Vector3(x, y, 0);
            }
        }
    }

    public void DestroyGrid()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
        tiles = null;
    }
    #endregion

    #region Stage Wave control
    public void InitStage()
    {
        stageState = StageState.Ongoing;
    }

    public void LoadWave()
    {

    }
    #endregion

    #region Monster


    /*
     * 몬스터 스폰
     * Wave, Coroutine으로 작성할 필요 있음
     * https://www.youtube.com/watch?v=r8N6J79W0go&ab_channel=Unity 참고
     */
    public bool SpawnMonster(Wave wave, int numOfWave)
    {
        for(int i = 0; i < wave.GetWaveBundle(numOfWave).numsOfMonster; i++)
        {
            // tiles[i * size + j] = (Tile)Instantiate(tilePrefab, new Vector3(i, j, 0), Quaternion.identity);
            // Spawning here
            var monster = (Monster)Instantiate<Monster>(wave.GetWaveBundle(numOfWave).monster, 
                new Vector3(Waypoints[0].x, Waypoints[0].y, 0), Quaternion.identity);
            monster.InitWaypoint(Waypoints);
        }
        // bool 
        return false;
    }
    #endregion
}

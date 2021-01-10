using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] private Vector2Int Size = new Vector2Int(0, 0);
    [SerializeField] private float waveStartWait = 0.0f;
    [SerializeField] private float monsterSpawnWait = 0.0f;
    [SerializeField] private Wave wave = null;
    [SerializeField] private Vector2Int[] Waypoints = null;

    public Tile[,] tiles;
    private int stageIdx = 0;
    private int waveIdx = 0;
    private List<Monster> monsters = new List<Monster>();

    float time = 0.0f;

    private void Start()
    {
        stageState = StageState.Ongoing;
        LoadWave(waveIdx);
    }

    private void Update()
    {
        // 시간 체크를 해야할 듯
        time += Time.deltaTime;
        if(time > 5 && stageState == StageState.Wait)
        {
            //Debug.Log("5초 초과");
            time = 0;
            //LoadWave(++waveIdx);
        }
    }

    // 보류
    public enum StageState
    {
        // 진행중, 실패, 성공, 언로드, 정지, 대기
        Ongoing, Fail, Success, Unload, Paused, Wait
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
    public void InitStage(int idx)
    {
        stageIdx = idx;
        stageState = StageState.Unload;
    }

    private void LoadWave(int index)
    {
        // 해당 index번째 Wave 코루틴 실행
        StartCoroutine(SpawnMonster(index));
    }

    public StageState GetState()
    {
        return stageState;
    }
    #endregion

    #region Monster

    /*
     * 몬스터 스폰
     * Wave, Coroutine으로 작성할 필요 있음
     * https://www.youtube.com/watch?v=r8N6J79W0go&ab_channel=Unity 참고
     * 
     */

    //타일 매니저로 생성시 참고한 코드
    //(Tile)Instantiate(tilePrefab, new Vector3(i, j, 0), Quaternion.identity);
    //Spawning here
    //Spawning position will be set StartPoint(x, y).Quaternion
    //tiles[i * size + j].name = "Tile[" + i + "][" + j + "]";
    //tiles[i * size + j].transform.parent = this.transform;

    private IEnumerator SpawnMonster(int waveIdx)
    {
        yield return new WaitForSeconds(waveStartWait);
        stageState = StageState.Ongoing;
        for (int i = 0; i < wave.GetWaveBundle(waveIdx).numOfMonster; i++)
        {
            var monster = Instantiate(wave.GetWaveBundle(waveIdx).monster, 
                                        new Vector3(Waypoints[0].x, Waypoints[0].y, 0), Quaternion.identity);

            if (monster != null)
            {
                monster.name = wave.GetWaveBundle(waveIdx).monster.ToString() + "" + monsters.Count;
                monster.transform.parent = this.transform;

                monster.InitWaypoint(Waypoints);
                monsters.Add(monster);
            }
            yield return new WaitForSeconds(monsterSpawnWait);
        }
        stageState = StageState.Wait;
    }

    // 스테이지 위치값에 따른 Waypoints에 offset 추가
    public void SpawnPointOffset() 
    {
        Vector2Int offset = new Vector2Int((int)gameObject.transform.position.x, (int)gameObject.transform.position.y);

        for (int i = 0; i < Waypoints.Length; i++)
        {
            Waypoints[i] += offset;
        }

    }
    #endregion



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// for debugging
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    [Header("Stage")]
    [SerializeField] private Vector2Int _size = new Vector2Int(0, 0);
    [SerializeField] private float _firstSpawnDelay = 0.0f;
    [SerializeField] private float _monsterSpawnDelay = 0.0f;
    [SerializeField] private Wave _wave = null;
    [SerializeField] private Vector2Int[] _waypoints = null;
    [SerializeField] private float _nextWaveDelay = 30.0f;

    public Tile[,] _tiles;
    private StageManager _stageManager;
    private int _stageIdx = 0;
    private int _waveIdx = 0;
    private List<Monster> _monsters = new List<Monster>();

    private float time = 0.0f;
    private Text _timerText;

    private void Start()
    {
        if (_stageIdx != -1)
        {
            _stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();

            LoadWave(_waveIdx);
        }
        else
        {
            _stageState = StageState.Unload;
        }

        InvokeRepeating("UpdateMonsterList", 0.0f, 2.0f);
    }

    private void Update()
    {
        if (_stageState == StageState.Ongoing)
        {
            _timerText = GameObject.Find("Timer").GetComponent<Text>();
            _timerText.text = "경과 시간:" + time + " \n현재 웨이브: " + _waveIdx + " 총 웨이브: " + _wave.GetNumOfWave()
                                        + "\n 남은 몬스터 수: " + _monsters.Count;
            time += Time.deltaTime;
            if (time > _nextWaveDelay && _waveIdx < _wave.GetNumOfWave())
            {
                time = 0;
                LoadWave(_waveIdx++);
            }
        }

        if (_waveIdx == _wave.GetNumOfWave() && _monsters.Count < 1)
        {
            _stageState = StageState.Success;
        }
    }

    public enum StageState
    {
        // 진행중, 실패, 성공, 언로드, 정지, 대기
        Ongoing, Fail, Success, Unload, Paused, Wait
    }
    private StageState _stageState { get; set; }


    #region Grid
    public void CreateGrid()
    {
        _tiles = new Tile[_size.x, _size.y];
        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                var tileGameObject = new GameObject($"Tile ({x}, {y})");
                var tile = tileGameObject.AddComponent<Tile>();

                _tiles[x, y] = tile;
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
        _tiles = null;
    }
    #endregion

    #region Stage Wave control
    public void InitStage(int idx)
    {
        _stageIdx = idx;
    }

    private void LoadWave(int index)
    {
        // 해당 index번째 Wave 코루틴 실행
        StartCoroutine(SpawnMonster(index));
    }

    public StageState GetState()
    {
        return _stageState;
    }
    #endregion

    #region Monster

    /*
     * 몬스터 스폰
     * https://www.youtube.com/watch?v=r8N6J79W0go&ab_channel=Unity 참고함
     */
    private IEnumerator SpawnMonster(int idx)
    {
        yield return new WaitForSeconds(_firstSpawnDelay);
        _stageState = StageState.Ongoing;
        for (int i = 0; i < _wave.GetWaveBundle(idx).numOfMonster; i++)
        {
            var monster = Instantiate(_wave.GetWaveBundle(idx).monster,
                                        new Vector3(_waypoints[0].x, _waypoints[0].y, 0), Quaternion.identity);

            if (monster != null)
            {
                monster.name = _wave.GetWaveBundle(idx).monster.ToString() + "" + _monsters.Count;
                monster.transform.parent = transform;

                monster.InitWaypoint(_waypoints);
                _monsters.Add(monster);
            }
            yield return new WaitForSeconds(_monsterSpawnDelay);
        }
    }

    // 스테이지 위치값에 따른 Waypoints에 offset 추가
    public void SpawnPointOffset()
    {
        Vector2Int offset = new Vector2Int((int)gameObject.transform.position.x, (int)gameObject.transform.position.y);

        for (int i = 0; i < _waypoints.Length; i++)
        {
            _waypoints[i] += offset;
        }

    }

    // 몬스터 리스트 업데이트 2초마다 실행됨
    void UpdateMonsterList()
    {
        for (int i = 0; i < _monsters.Count; i++)
        {
            if (_monsters[i] == null)
            {
                _monsters.Remove(_monsters[i]);
            }
        }
    }
    #endregion



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    [Header("Stage")]
    [SerializeField] private Vector2Int _size = new Vector2Int(0, 0);
    [SerializeField] private Wave _wave = null;
    [SerializeField] private Vector2[] _waypoints = null;
    [SerializeField] private float _nextWaveDelay = 30.0f;
    [SerializeField] private uint _earningMoney = 0;

    [Header("Monster")]
    [SerializeField] private float _firstSpawnDelay = 0.0f;
    [SerializeField] private float _monsterSpawnDelay = 0.0f;

    [SerializeField] private Tile[,] _tiles;
    private StageManager _stageManager;

    private int _stageIdx = -1;
    private int _waveIdx = 0;
    private List<Monster> _monsters = new List<Monster>();

    private float time = 0.0f;
    private Text _timerText;
    private System.Text.StringBuilder stringBuilder;

    public enum StageState
    {
        Ongoing, Fail, Success
    }
    private StageState _stageState { get; set; }

    private void Awake()
    {
        stringBuilder = new System.Text.StringBuilder("");
    }

    private void Start()
    {
        if (_stageIdx < 0)
        {
            _stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
            _stageState = StageState.Ongoing;
        }

        _timerText = GameObject.Find("Timer").GetComponent<Text>();

        InvokeRepeating("UpdateState", 0.0f, 0.1f);
        InvokeRepeating("CheckStageState", _nextWaveDelay + _monsterSpawnDelay, 1.0f);
    }

    private void Update()
    {
        time += Time.deltaTime;
    }

    private void UpdateState()
    {
        if (_stageState == StageState.Ongoing)
        {
            if (_wave.GetNumOfWave() != _waveIdx)
            {
                stringBuilder.Clear();
                stringBuilder.Append("Next Wave: " + (_nextWaveDelay - Mathf.Round(time)));
                stringBuilder.Append("\nNow/Total: " + _waveIdx + "/" + _wave.GetNumOfWave());
            }
            else
            {
                stringBuilder.Clear();
                stringBuilder.Append("Last Wave");
            }

            stringBuilder.Append("\nMonsters: " + _monsters.Count);
            stringBuilder.Append("\nHP: " + Player.GetInstance().GetLife());
            stringBuilder.Append("\nMoney: " + Player.GetInstance().GetMoney());
            stringBuilder.Append("\nGame speed: " + Mathf.FloorToInt(Time.timeScale));

            _timerText.text = stringBuilder.ToString();

            if (_waveIdx == 0)
            {
                time = 0;
                LoadWave(_waveIdx++);
                return;
            }

            if (time > _nextWaveDelay && _waveIdx < _wave.GetNumOfWave())
            {
                time = 0;
                LoadWave(_waveIdx++);
            }

            if (Player.GetInstance().GetLife() <= 0)
            {
                _stageState = StageState.Fail;
            }
        }
    }

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

    // Wave 시작
    private void LoadWave(int index)
    {
        StartCoroutine(SpawnMonster(index));
        Player.GetInstance().AddMoney((int)_earningMoney);
    }

    public StageState GetState()
    {
        return _stageState;
    }

    public void SetState(StageState State)
    {
        _stageState = State;
    }
    #endregion

    #region Monster
    private IEnumerator SpawnMonster(int idx)
    {
        yield return new WaitForSeconds(_firstSpawnDelay);
        _stageState = StageState.Ongoing;
        for (int i = 0; i < _wave.GetWaveBundle(idx).numOfMonster; i++)
        {
            var monster =
                Instantiate(_wave.GetWaveBundle(idx).monster,
                            new Vector3(_waypoints[0].x, _waypoints[0].y, 0),
                            Quaternion.identity);

            if (monster != null)
            {
                monster.transform.SetParent(gameObject.transform);

                monster.InitWaypoint(_waypoints);
                _monsters.Add(monster);
            }
            yield return new WaitForSeconds(_monsterSpawnDelay);
        }
    }

    public void SpawnPointOffset()
    {
        Vector2 offset = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);

        for (int i = 0; i < _waypoints.Length; i++)
        {
            _waypoints[i] += offset;
        }
    }

    void CheckStageState()
    {
        for (int i = 0; i < _monsters.Count; i++)
        {
            if (_monsters[i] == null)
            {
                _monsters.Remove(_monsters[i]);
            }
        }

        if ((_waveIdx == _wave.GetNumOfWave()) && _monsters.Count < 1)
        {
            _stageState = StageState.Success;
        }
    }
    #endregion
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
    [Header("Stage")]
    [SerializeField] private Vector2Int _size = new Vector2Int(0, 0);
    [SerializeField] private Wave _wave = null;
    [SerializeField] private Vector2Int[] _waypoints = null;
    [SerializeField] private float _nextWaveDelay = 30.0f;

    [Header("Monster")]
    [SerializeField] private float _firstSpawnDelay = 0.0f;
    [SerializeField] private float _monsterSpawnDelay = 0.0f;

    public Tile[,] _tiles;
    private StageManager _stageManager;
    // _stageIdx는 로드가 진행되지 않으면 -1값임
    private int _stageIdx = -1;
    private int _waveIdx = 0;
    private List<Monster> _monsters = new List<Monster>();

    private float time = 0.0f;
    private Text _timerText;

    private void Start()
    {
        // 초기화로 _stageIdx가 유효한 값이면
        if (_stageIdx < 0)
        {
            _stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
            _stageState = StageState.Ongoing;
        }

        // StageState를 검사하는 함수. 맨 처음 실행 전 _nextWaveDelay와 _monsterSpawnDelay를 기다림
        InvokeRepeating("CheckStageState", _nextWaveDelay + _monsterSpawnDelay, 1.0f);
    }

    private void Update()
    {
        if (_stageState == StageState.Ongoing)
        {
            _timerText = GameObject.Find("Timer").GetComponent<Text>();
            _timerText.text = "경과 시간 : " + Mathf.Round(time) + " \n현재 웨이브/총 웨이브 수 : " + _waveIdx + "/" + _wave.GetNumOfWave()
                                        + "\n남은 몬스터 수 : " + _monsters.Count
                                        + "\n[현재 체력: " + Player.GetInstance().GetLife() + "][현재 금액: " + Player.GetInstance().GetMoney() + "]";
            time += Time.deltaTime;

            // 맨 처음 웨이브라면 _nextWaveDelay 기다리지 않음
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
    // StageManager에서 스테이지를 인덱스를 받아 초기화
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
    * 웨이브 진행 중 몬스터 스폰
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
                monster.transform.SetParent(gameObject.transform);

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

    // 몬스터 리스트 업데이트와 몬스터의 수와 웨이브 수를 체크
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

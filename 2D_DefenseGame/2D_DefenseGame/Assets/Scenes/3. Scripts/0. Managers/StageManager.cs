﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 스테이지의 생성과 파괴를 관리
public class StageManager : MonoBehaviour
{
    [Header("Stage")]
    [SerializeField] private Stage[] _stages = null;
    [SerializeField] public Vector3 _stageHolder;
    [SerializeField] private GameObject _gameOverPanel = null;
    [SerializeField] private GameObject _pausePanel = null;

    private GameObject _stageDataHolder;
    private Stage _currentStage;
    private int _currStageIdx = -1;
    private bool _bIsScene { get; set; }

    private void Awake()
    {
        // for Debugging 일단 게임씬에서 실행할때 에러 안뜨도록 추가
        if (_stageDataHolder == null)
        {
            GameObject stageHolder = new GameObject("StageDataHolder");
            stageHolder.AddComponent<StageDataHolder>();
            _stageDataHolder = stageHolder;
        }
    }

    void Start()
    {
        _stageDataHolder = GameObject.Find("StageDataHolder");

        _currStageIdx = _stageDataHolder.GetComponent<StageDataHolder>().GetCurrentStage();
        //SetCurrentStage(_currStageIdx);
        SetCurrentStage(1);
    }

    void Update()
    {
        if (_currentStage != null)
        {
            if (_bIsScene)
            {
                CancelInvoke("CheckStageLoaded");
            }

            if (_currentStage.GetState() == Stage.StageState.Success)
            {
                Destroy(_currentStage, 1.0f);
                if(_currStageIdx >= PlayerPrefs.GetInt("StageCount"))
                {
                    _stageDataHolder.GetComponent<StageDataHolder>().AddClearCount();
                }
                SceneManager.LoadScene("StageSelectScene");
            }
            else if (_currentStage.GetState() == Stage.StageState.Fail)
            {
                Time.timeScale = 0;
                _gameOverPanel.SetActive(true);
                _currentStage.SetState(Stage.StageState.Paused);
            }
        }
    }

    #region Stage Setting & Load
    public void LoadStage(int index)
    {
        _currentStage = Instantiate<Stage>(_stages[index], _stageHolder, Quaternion.identity);

        if (_currentStage != null)
        {
            _currentStage.SpawnPointOffset();
            _stages[index].InitStage(index);
        }
    }

    private void CheckStageLoaded()
    {
        if (_bIsScene && _currentStage == null)
        {
            LoadStage(_currStageIdx);
            _bIsScene = false;
            CancelInvoke("CheckInScene");
        }
    }

    public void SetCurrentStage(int index)
    {
        _currStageIdx = index;
        InvokeRepeating("CheckStageLoaded", 0.2f, 2f);
        _bIsScene = true;
    }

    #endregion

    public void ReloadStage()
    {
        Destroy(_currentStage.gameObject);

        LoadStage(_currStageIdx);
        Player.GetInstance().ResetGame();
        Time.timeScale = 1;
        _gameOverPanel.SetActive(false);
        _pausePanel.SetActive(false);
    }

    public void QuitToStageSelectorScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StageSelectScene");
    }

    public void OpenPauseMenu()
    {
        if(!_gameOverPanel.activeInHierarchy)
        {
            Time.timeScale = 0;
            _pausePanel.SetActive(true);
        }
    }

    public void GoOn()
    {
        Time.timeScale = 1;
        _pausePanel.SetActive(false);
    }

}

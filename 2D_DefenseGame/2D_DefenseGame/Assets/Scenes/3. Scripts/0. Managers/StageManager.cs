using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [Header("Stage")]
    [SerializeField] private Stage[] _stages = null;
    [SerializeField] private Vector3 _stageHolder = Vector3.zero;
    [SerializeField] private GameObject _gameOverPanel = null;
    [SerializeField] private GameObject _pausePanel = null;
    [SerializeField] private GameObject _clearPanel = null;

    private GameObject _stageDataHolder;
    private Stage _currentStage;
    private int _currStageIdx = -1;
    private bool _isAtScene { get; set; }

    private void Awake()
    {
        if (_stageDataHolder == null)
        {
            GameObject stageHolder = new GameObject("StageDataHolder");
            stageHolder.AddComponent<StageDataHolder>();
            _stageDataHolder = stageHolder;
        }
    }

    private void Start()
    {
        _stageDataHolder = GameObject.Find("StageDataHolder");

        _currStageIdx = _stageDataHolder.GetComponent<StageDataHolder>().GetCurrentStage();
        SetCurrentStage(_currStageIdx);

        InvokeRepeating("UpdateStageManager", 0.0f, 0.1f);
    }

    private void UpdateStageManager()
    {
        if (_currentStage.GetState() == Stage.StageState.Ongoing)
        {
            return;
        }
        else if (_currentStage.GetState() == Stage.StageState.Success)
        {
            Player.GetInstance().AddStageClearCount(_currStageIdx);
            _stageDataHolder.GetComponent<StageDataHolder>().SetCurrentStage(_currStageIdx + 1);
            OpenClearMenu();
        }
        else if (_currentStage.GetState() == Stage.StageState.Fail)
        {
            Time.timeScale = 0;
            _gameOverPanel.SetActive(true);
            _stageDataHolder.GetComponent<StageDataHolder>().SetCurrentStage(_currStageIdx);
            CancelInvoke("UpdateStageManager");
        }
    }

    #region Stage Setting & Load

    private void LoadStage(int index)
    {
        if(!IsInvoking("UpdateStageManager"))
        {
            InvokeRepeating("UpdateStageManager", 0.0f, 0.1f);
        }

        _currentStage = Instantiate<Stage>(_stages[index], _stageHolder, Quaternion.identity);

        if (_currentStage != null)
        {
            _stages[index].InitStage(index);
        }
    }
    
    private void SetCurrentStage(int index)
    {
        _currStageIdx = index;
        _isAtScene = true;
        InvokeRepeating("CheckStageLoaded", 0.0f, 0.1f);
    }

    private void CheckStageLoaded()
    {
        if (_isAtScene && (_currentStage == null))
        {
            LoadStage(_currStageIdx);
            _isAtScene = false;
            CancelInvoke("CheckInScene");
        }
    }

    #endregion

    #region Stage interaction

    public void ReloadStage()
    {
        if(_currentStage != null)
        {
            Transform[] objs = _currentStage.GetComponentsInChildren<Transform>();
            foreach(Transform obj in objs)
            {
                Destroy(obj.gameObject);
            }
            Destroy(_currentStage.gameObject);
        }

        LoadStage(_currStageIdx);
        Player.GetInstance().ResetGame();
        Time.timeScale = 1;

        _gameOverPanel.SetActive(false);
        _pausePanel.SetActive(false);
        _clearPanel.SetActive(false);
    }

    public void QuitToStageSelectorScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StageSelectScene");
    }

    public void OpenPauseMenu()
    {
        if (!_gameOverPanel.activeInHierarchy)
        {
            Time.timeScale = 0;
            _pausePanel.SetActive(true);
        }
    }

    public void OpenClearMenu()
    {
        CancelInvoke("UpdateStageManager");
        if (!_gameOverPanel.activeInHierarchy)
        {
            Time.timeScale = 0;
            _clearPanel.SetActive(true);

            if ((_currStageIdx + 1) == Player.GetInstance()._numOfStage)
            {
                Button[] btns = _clearPanel.GetComponentsInChildren<Button>();
                foreach (Button btn in btns)
                {
                    if (btn.name.Equals("NextStage"))
                    {
                        btn.gameObject.SetActive(false);
                        break;
                    }
                }
            }
        }
    }

    public void LoadNextStage()
    {
        _currStageIdx++;
        ReloadStage();
    }

    public void GoOn()
    {
        Time.timeScale = 1;
        _pausePanel.SetActive(false);
    }

    #endregion
}

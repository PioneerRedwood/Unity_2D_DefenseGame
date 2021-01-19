using System.Collections;
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

    private GameObject _stageDataHolder;
    private Stage _currentStage;
    private int _currStageIdx = -1;
    private bool _bIsScene { get; set; }

    void Start()
    {
        _stageDataHolder = GameObject.Find("StageDataHolder");
        // for Debugging 일단 게임씬에서 실행할때 에러 안뜨도록 추가
        if (_stageDataHolder == null)
        {
            GameObject stageHolder = new GameObject("StageDataHolder");
            stageHolder.AddComponent<StageDataHolder>();
            _stageDataHolder = stageHolder;
        }

        _currStageIdx = _stageDataHolder.GetComponent<StageDataHolder>().GetCurrentStage();
        SetCurrentStage(_currStageIdx);
    }

    void Update()
    {
        if (_currentStage != null)
        {
            if (_bIsScene)
            {
                CancelInvoke("CheckStageLoaded");
            }

            // 스테이지 클리어시 해야하는 동작 추가 바람
            if (_currentStage.GetState() == Stage.StageState.Success)
            {
                Destroy(_currentStage, 1.0f);
                if(_currStageIdx >= PlayerPrefs.GetInt("StageCount"))
                {
                    _stageDataHolder.GetComponent<StageDataHolder>().AddClearCount();
                }
                SceneManager.LoadScene("StageSelectScene");
                Debug.Log("Stage Clear!" + PlayerPrefs.GetInt("StageCount"));
            }
            else if (_currentStage.GetState() == Stage.StageState.Fail)
            {
                Debug.Log("Stage Fail!");
                Destroy(_currentStage, 1.0f);
                SceneManager.LoadScene("StageSelectScene");
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

    // 해당 함수는 SetCurrentStage()에서 참조됨
    // 시작시 0.2초 딜레이 후 2초마다 실행 후 InvokeRepeating()을 종료
    // 씬에 존재할 때 스테이지가 로드되지 않았다면 로드
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
}

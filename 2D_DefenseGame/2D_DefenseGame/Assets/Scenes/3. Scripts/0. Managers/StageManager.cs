using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 스테이지의 생성과 파괴를 관리
public class StageManager : MonoBehaviour
{
    [SerializeField] private Stage[] _stages = null;
    [SerializeField] public Vector3 _stageHolder;

    private StageContentViewer stageViewer;
    private StageSelectCanvas stageSelectCanvas;
    private Stage _currentStage;
    private int _currStageIdx = -1;
    private bool bIsScene { get; set; }
    
    void Start()
    {
        // 씬을 바꾸니 두개가 존재하는 현상이 발생함
        stageViewer = GameObject.Find("Content").GetComponent<StageContentViewer>();
        stageSelectCanvas = GameObject.Find("StageSelectCanvas").GetComponent<StageSelectCanvas>();

        DontDestroyOnLoad(gameObject);
    }

    private void CheckInScene()
    {
        if (bIsScene && _currentStage == null)
        {
            LoadStage(_currStageIdx);
            bIsScene = false;
            CancelInvoke("CheckInScene");
        }
    }

    void Update()
    {
        if (_currentStage != null)
        {
            if (bIsScene)
            {
                CancelInvoke("CheckInScene");
            }

            if (_currentStage.GetState() == Stage.StageState.Success)
            {
                Destroy(_currentStage, 1.0f);
                stageViewer.UpdateNextStage();
                stageSelectCanvas.SetActive(true);
                SceneManager.LoadScene("StageSelectScene");
            }
            else if (_currentStage.GetState() == Stage.StageState.Fail)
            {
                Destroy(_currentStage, 1.0f);
                stageSelectCanvas.SetActive(true);
                SceneManager.LoadScene("StageSelectScene");
            }
        }
    }

    public void SetCurrentStage(int index)
    {
        _currStageIdx = index;
        InvokeRepeating("CheckInScene", 0.2f, 2f);
        bIsScene = true;

        SceneManager.LoadScene("SampleScene");
    }

    public void LoadStage(int index)
    {
        _currentStage = Instantiate<Stage>(_stages[index], _stageHolder, Quaternion.identity);
        if(_currentStage != null)
        {
            _currentStage.SpawnPointOffset();

            _stages[index].InitStage(index);
            
        }
    }

}

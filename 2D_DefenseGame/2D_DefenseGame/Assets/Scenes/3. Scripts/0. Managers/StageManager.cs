using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 스테이지의 생성과 파괴를 관리
public class StageManager : MonoBehaviour
{
    [SerializeField] private Stage[] _stages = null;
    [SerializeField] public Vector3 _stageHolder;
    

    private Stage _currentStage;
    private int _bIsWaveOngoing { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        // LoadStage() will be activated by clicking stage open button
        // -- for test --
        LoadStage(0);
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    public void LoadStage(int index)
    {
        _currentStage = Instantiate<Stage>(_stages[index], _stageHolder, Quaternion.identity);
        _currentStage.SpawnPointOffset();
       
        _stages[index].InitStage(index);
        _bIsWaveOngoing = index;
        // 게임 UI 변경

    }

    public void UnloadStage(int index)
    {
        _stages[index].InitStage(index);
        Destroy(_currentStage);
    }
}

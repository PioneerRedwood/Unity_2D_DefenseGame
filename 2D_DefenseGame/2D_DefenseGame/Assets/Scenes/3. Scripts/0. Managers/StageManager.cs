using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스테이지의 생성과 파괴를 관리
public class StageManager : MonoBehaviour
{
    [SerializeField] private Stage[] stages;
    [SerializeField] public Vector3 stageHolder;

    private Stage currentStage;
    private bool bIsWaveOngoing { get; set; }


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
        currentStage = (Stage)Instantiate<Stage>(stages[index], stageHolder, Quaternion.identity);
        stages[index].InitStage(index);
        bIsWaveOngoing = true;
        // 게임 UI 변경

    }

    public void UnloadStage(int index)
    {
        stages[index].InitStage(index);
        Destroy(currentStage);
    }
}

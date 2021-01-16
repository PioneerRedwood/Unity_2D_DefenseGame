using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 씬 이동시 필요한 스테이지에 대한 정보 래퍼 클래스
public class StageDataHolder : MonoBehaviour
{
    private int _currStageIdx = 0;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public int GetCurrentStage()
    {
        return _currStageIdx;
    }

    public void SetCurrentStage(int set)
    {
        _currStageIdx = set;
    }
}

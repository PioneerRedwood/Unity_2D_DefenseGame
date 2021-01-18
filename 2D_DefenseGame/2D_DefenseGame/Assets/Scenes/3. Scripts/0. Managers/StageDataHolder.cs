using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 씬 이동시 필요한 스테이지에 대한 정보 래퍼 클래스
public class StageDataHolder : MonoBehaviour
{
    private int _currStageIdx = 0;
    private int _clearCount = 0;

    private void Start()
    {
        var tempObject = FindObjectsOfType<StageDataHolder>();
        if(tempObject.Length == 1)
        {
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public int GetCurrentStage()
    {
        return _currStageIdx;
    }

    public void SetCurrentStage(int set)
    {
        _currStageIdx = set;
    }

    public void AddClearCount()
    {
        _clearCount++;
        PlayerPrefs.SetInt("StageCount", _clearCount);
    }

    public int GetClearCount()
    {
        return _clearCount;
    }
}

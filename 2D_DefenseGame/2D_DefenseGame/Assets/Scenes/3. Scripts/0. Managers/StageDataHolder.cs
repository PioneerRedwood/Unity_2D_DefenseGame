using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDataHolder : MonoBehaviour
{
    private int _currStageIdx = 0;

    private void Start()
    {
        var tempObject = FindObjectsOfType<StageDataHolder>();

        if (tempObject.Length == 1)
        {
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
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
}

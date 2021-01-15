using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageViewer : MonoBehaviour
{
    private int _idx = -1;
    private StageManager _stageManager;
    private GameObject _stageSelectCanvas;

    void Start()
    {
        _stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
        _stageSelectCanvas = GameObject.Find("StageSelectCanvas");
    }

    void Update()
    {
        
    }

    public void SetStageIndex(int idx)
    {
        _idx = idx;
    }

    public void OnStageClicked()
    {
        if (_idx != -1)
        {
            _stageManager.SetCurrentStage(_idx);
            _stageSelectCanvas.SetActive(false);
        }

    }
}

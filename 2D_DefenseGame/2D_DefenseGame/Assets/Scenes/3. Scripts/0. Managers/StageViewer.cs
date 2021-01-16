using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StageViewer : MonoBehaviour
{
    private int _idx = -1;

    public void SetStageIndex(int idx)
    {
        _idx = idx;
    }

    public void OnStageClicked()
    {
        if (_idx != -1)
        {
            SceneManager.LoadScene("GameScene");
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class StageViewer : MonoBehaviour
{
    [Header("Stage UI")]
    [SerializeField] private Text _stateText = null;
    [SerializeField] private Image _image = null;

    private int _idx = -1;

    public enum StageViewState
    {
        NotTried, Cleared, Locked
    };
    public StageViewState _state = StageViewState.NotTried;

    public void SetStageIndex(int idx)
    {
        _idx = idx;
    }

    public void OnStageClicked()
    {
        // 배포 시 없앨 것
        //if (_idx != -1 && (_state != StageViewState.Locked))
        if (_idx != -1)
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    public void InitStageViewer(StageViewState idx)
    {
        Color tempColor = _image.color;
        
        switch (idx)
        {
            case StageViewState.Cleared:

                _stateText.text = "Clear";
                
                tempColor.a = 0.2f;
                _image.color = tempColor;

                _state = StageViewState.Cleared;

                break;
            case StageViewState.Locked:

                //_stateText.text = "Locked";
                _stateText.text = "Debuggin On";

                tempColor.a = 0.4f;
                _image.color = tempColor;

                _state = StageViewState.Locked;

                break;
            case StageViewState.NotTried:
                break;
            default:
                break;
        }
    }
}

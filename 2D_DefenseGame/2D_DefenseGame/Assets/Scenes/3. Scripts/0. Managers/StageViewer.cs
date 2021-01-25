using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class StageViewer : MonoBehaviour
{
    [Header("Stage UI")]
    public Text _text;
    public Image _image;

    private int _idx = -1;

    public enum StageViewState
    {
        NotTried, Cleared, Failed
    };
    public StageViewState _state = StageViewState.NotTried;

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

    // 스테이지 성공 / 실패 시 UI 변경
    public void InitStageViewer(StageViewState idx)
    {
        if (idx == StageViewState.Cleared)
        {
            // text
            _text.text = "Clear";

            // color
            Color tempColor = _image.color;
            tempColor.a = 0.2f;
            _image.color = tempColor;

            _state = StageViewState.Cleared;
        }
        // 클리어 말곤 아무 반응 없음
        else if(idx == StageViewState.Failed)
        {
            _text.text = "Failed";

            Color tempColor = _image.color;
            tempColor.a = 0.2f;
            _image.color = tempColor;

            _state = StageViewState.Failed;
        }
        else
        {
            // 아직 시도하지 않은 스테이지일때 표시
        }
    }
}

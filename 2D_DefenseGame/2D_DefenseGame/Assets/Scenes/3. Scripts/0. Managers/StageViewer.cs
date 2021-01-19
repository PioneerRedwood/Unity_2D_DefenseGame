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
    private bool _isClear = false;

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

    public void OnClearStage()
    {
        if (!_isClear)
        {
            // text
            _text.text = "Clear";

            // color
            Color tempColor = _image.color;
            tempColor.a = 0.2f;
            _image.color = tempColor;

            _isClear = true;
        }
    }
}

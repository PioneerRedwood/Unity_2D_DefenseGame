using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class StageViewer : MonoBehaviour
{
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
            Text[] tempTexts = gameObject.GetComponentsInChildren<Text>(true);
            foreach (Text text in tempTexts)
            {
                if (text.name == "ClearText")
                {
                    text.text = "Clear";
                }
            }
            Image[] tempImgs = gameObject.GetComponentsInChildren<Image>();
            foreach(Image image in tempImgs)
            {
                if(image.name == "Image")
                {
                    Color tempColor = image.color;
                    tempColor.a = 0.25f;
                    image.color = tempColor;
                }
            }

            _isClear = true;
        }
    }
}

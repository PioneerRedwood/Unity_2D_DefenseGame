using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Stage Select UI
public class StageSelector : MonoBehaviour
{
    [Header("Stage")]
    public StageViewer[] _stageVieweres;
    public GameObject _content;
    public StageDataHolder _stageDataHolder;
    public Text _textCurrentIndex;

    private float _speed = 40.0f;

    private enum OnMove { Stop, Right, Left };
    private OnMove _onMove = OnMove.Stop;
    private float _nextXPos = 0;

    // -1은 세팅이 안된 상태
    private int _currIdx = 0;
    private int _prevIdx = -1;
    private int _nextIdx = -1;

    void Start()
    {
        Time.timeScale = 1;
        _content.GetComponent<HorizontalLayoutGroup>().spacing = Screen.width;

        if (GameObject.Find("StageDataHolder") == null)
        {
            GameObject stageHolder = new GameObject("StageDataHolder");
            stageHolder.AddComponent<StageDataHolder>();
            _stageDataHolder = stageHolder.GetComponent<StageDataHolder>();
        }
        else
        {
            _stageDataHolder = GameObject.Find("StageDataHolder").GetComponent<StageDataHolder>();
        }
        
        OnSelectorReload();
    }

    private void Update()
    {
        if (_onMove != OnMove.Stop)
        {
            _content.transform.position =
                Vector2.Lerp(new Vector2(_content.transform.position.x, _content.transform.position.y), new Vector2(_nextXPos, _content.transform.position.y), _speed * Time.deltaTime);

            if (Mathf.Approximately(_content.transform.position.x, _nextXPos))
            {
                _onMove = OnMove.Stop;
            }
        }
    }

    private void OnSelectorReload()
    {
        int idx = 0;
        // _stageDataHolder.GetClearCount() 는 PlayerPrefs.GetInt("StageCount")에 저장됨

        foreach (StageViewer viewer in _stageVieweres)
        {
            var tempViewer = Instantiate(viewer);
            tempViewer.transform.SetParent(_content.transform);

            // 전에 클리어한 부분 있으면 Clear한 것 표시
            if (idx < PlayerPrefs.GetInt("StageCount"))
            {
                tempViewer.InitStageViewer(StageViewer.StageViewState.Cleared);
            }
            tempViewer.SetStageIndex(idx++);
        }

        if (PlayerPrefs.GetInt("StageCount") == 0)
        {
            _currIdx = 0;
            _nextIdx = 1;
            _prevIdx = -1;
            SetContentPosition(-1);
        }
        else
        {
            _currIdx = PlayerPrefs.GetInt("StageCount");
            _nextIdx = _currIdx + 1;
            _prevIdx = _currIdx - 1;

            SetContentPosition(PlayerPrefs.GetInt("StageCount"));
        }
        _textCurrentIndex.text = _currIdx + 1 + " / " + _stageVieweres.Length;
    }

    private void SetContentPosition(int idx)
    {
        float x = 0;
        if (idx == -1)
        {
            // 초기 컨텐츠 위치
            if (_stageVieweres.Length % 2 == 0)
            {
                x = Screen.width * (_stageVieweres.Length / 2) - (Screen.width / 2);
            }
            else
            {
                x = Screen.width * (_stageVieweres.Length / 2);
            }
        }
        else
        {
            // 클리어 후
            if (_stageVieweres.Length % 2 == 0)
            {
                x = (Screen.width * (_stageVieweres.Length / 2) - (Screen.width / 2)) - (idx * Screen.width);
            }
            else
            {
                x = (Screen.width * (_stageVieweres.Length / 2)) - (idx * Screen.width);
            }
        }
        _content.transform.localPosition = new Vector3(x, 0.0f, 0.0f);
    }

    #region UI Button
    public void OnMoveContentLeftButtonClicked()
    {
        // 버튼이 여러번 눌려도 움직이려는 위치까지 이동하지 않았다면 실행 X
        if (_currIdx > 0 && _onMove == OnMove.Stop)
        {
            _nextXPos = _content.transform.position.x + Screen.width;
            --_currIdx;
            _nextIdx = _currIdx + 1;
            _prevIdx = _currIdx - 1;
            _onMove = OnMove.Left;
            _stageDataHolder.SetCurrentStage(_currIdx);
            _textCurrentIndex.text = _currIdx + 1 + " / " + _stageVieweres.Length;
        }
    }

    public void OnMoveContentRightButtonClicked()
    {
        if (_currIdx < _stageVieweres.Length - 1 && _onMove == OnMove.Stop)
        {
            _nextXPos = _content.transform.position.x - Screen.width;
            ++_currIdx;
            _nextIdx = _currIdx + 1;
            _prevIdx = _currIdx - 1;
            _onMove = OnMove.Right;
            _stageDataHolder.SetCurrentStage(_currIdx);
            _textCurrentIndex.text = _currIdx + 1 + " / " + _stageVieweres.Length;
        }
    }

    public void OnGoToMenuButtonClicked()
    {
        SceneManager.LoadScene("StartScene");
    }
    #endregion
}

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

    [Header("General")]
    [SerializeField] private int _widthOffset = 1366;
    [SerializeField] private float _speed = 40.0f;

    private enum OnMove { Stop, Right, Left };
    private OnMove _onMove = OnMove.Stop;
    private float _nextXPos = 0;

    // -1은 세팅이 안된 상태
    private int _currIdx = 0;
    private int _prevIdx = -1;
    private int _nextIdx = -1;

    void Start()
    {
        _stageDataHolder = GameObject.Find("StageDataHolder").GetComponent<StageDataHolder>();
        OnSelectorReload();
    }

    private void Update()
    {
        if (_onMove != OnMove.Stop)
        {
            Debug.Log("이전: " + _prevIdx + " 현재: " + _currIdx + " 다음: " + _nextIdx);

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
        Debug.Log(PlayerPrefs.GetInt("StageCount"));

        foreach (StageViewer viewer in _stageVieweres)
        {
            var tempViewer = Instantiate(viewer);
            tempViewer.transform.SetParent(_content.transform);

            // 전에 클리어한 부분 있으면 Clear한 것 표시
            if (idx < PlayerPrefs.GetInt("StageCount"))
            {
                tempViewer.OnClearStage();
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
                x = _widthOffset * (_stageVieweres.Length / 2) - (_widthOffset / 2);
            }
            else
            {
                x = _widthOffset * (_stageVieweres.Length / 2);
            }
        }
        else
        {
            // 클리어 후
            if (_stageVieweres.Length % 2 == 0)
            {
                x = (_widthOffset * (_stageVieweres.Length / 2) - (_widthOffset / 2)) - (idx * _widthOffset);
            }
            else
            {
                x = (_widthOffset * (_stageVieweres.Length / 2)) - (idx * _widthOffset);
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
            _nextXPos = _content.transform.position.x + _widthOffset;
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
            _nextXPos = _content.transform.position.x - _widthOffset;
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

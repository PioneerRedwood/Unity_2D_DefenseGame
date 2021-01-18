using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stage Select UI
public class StageSelector : MonoBehaviour
{
    [Header("Stage")]
    public StageViewer[] _stageVieweres;
    public GameObject _content;
    public StageDataHolder _stageDataHolder;

    [Header("General")]
    [SerializeField] private int _widthOffset = 1366;
    [SerializeField] private float _speed = 15.0f;

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
        // 씬이 바뀌어도 viewer에는 꼭 넣어야..
        Debug.Log(_stageDataHolder.GetClearCount());

        foreach (StageViewer viewer in _stageVieweres)
        {
            var tempViewer = Instantiate(viewer);
            tempViewer.transform.SetParent(_content.transform);
            tempViewer.SetStageIndex(idx++);
            if(idx < _stageDataHolder.GetClearCount())
            {
                tempViewer.OnClearStage();
            }
        }

        if (_stageDataHolder.GetClearCount() == 0)
        {            
            SetContentPosition(-1);
            _currIdx = 0;
            _nextIdx = 1;
            _prevIdx = -1;
        }
        else
        {
            _currIdx = _stageDataHolder.GetClearCount();
            _nextIdx = _currIdx + 1;
            _prevIdx = _currIdx - 1;
            SetContentPosition(_stageDataHolder.GetClearCount());
        }
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
        Debug.Log(x);
        _content.transform.localPosition = new Vector3(x, 0.0f, 0.0f);
    }

    #region R/L Button
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
        }
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelector : MonoBehaviour
{
    [Header("Stage")]
    public StageViewer[] _stageViewer;
    public GameObject _content;
    public StageDataHolder _stageDataHolder;

    [Header("General")]
    [SerializeField] private int _widthOffset = 1366;
    [SerializeField] private float _speed = 15.0f;

    private enum OnMove { Stop, Right, Left };
    private OnMove _onMove = OnMove.Stop;
    private float _nextXPos = 0;

    // 현재, 이전 인덱스
    private int _currIdx = 0;
    private int _prevIdx = -1;
    private int _nextIdx = -1;

    // Start is called before the first frame update
    void Start()
    {
        _stageDataHolder = GameObject.Find("StageDataHolder").GetComponent<StageDataHolder>();

        int idx = 0;
        // 초기 생성시 
        if (_nextIdx < 0)
        {
            foreach (StageViewer viewer in _stageViewer)
            {
                var tempViewer = Instantiate(viewer);
                tempViewer.transform.SetParent(_content.transform);
                tempViewer.SetStageIndex(idx++);
            }
            _nextIdx++;
            _content.transform.localPosition = new Vector3(_widthOffset * (_stageViewer.Length / 2), 0.0f, 0.0f);
        }
        else
        {
            // 보여지는 인덱스는 클리어한 스테이지 바로 다음 스테이지로 설정
            // localPosition의 범위: 배열의 크기가 5일 경우 -2 x 1366(_widthOffset) ~ 2 x 1366(_widthOffset)
            // _nextIdx에는 현재 스테이지 바로 다음 스테이지 수가 들어감 활용해보기
            _content.transform.localPosition = new Vector3(_widthOffset * _nextIdx, 0.0f, 0.0f);
        }

    }


    private void Update()
    {
        // 보여지는 컨텐츠 위치 선형 보간 변경
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

    public void OnMoveContentLeftButtonClicked()
    {
        // 버튼이 여러번 눌려도 움직이려는 위치까지 이동하지 않았다면 실행 X
        if (_currIdx > 0 && _onMove == OnMove.Stop)
        {
            _nextXPos = _content.transform.position.x + _widthOffset;
            --_currIdx;
            --_prevIdx;
            --_nextIdx;
            _onMove = OnMove.Left;
            UpdateCurrStage();
        }
    }

    public void OnMoveContentRightButtonClicked()
    {
        if (_currIdx < _stageViewer.Length - 1 && _onMove == OnMove.Stop)
        {
            _nextXPos = _content.transform.position.x - _widthOffset;
            ++_currIdx;
            ++_prevIdx;
            ++_nextIdx;
            _onMove = OnMove.Right;
            UpdateCurrStage();
        }
    }

    public void UpdateCurrStage()
    {
        _stageDataHolder.SetCurrentStage(_currIdx);
    }

}

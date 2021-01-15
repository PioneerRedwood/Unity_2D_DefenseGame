using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageContentViewer : MonoBehaviour
{
    public StageViewer[] _stageViewer;
    private enum OnMove { Stop, Right, Left };
    private OnMove _onMove = OnMove.Stop;
    private float _nextXPos = 0;
    private int _widthOffset = 1366;
    private float _speed = 15.0f;

    // 현재, 이전 인덱스
    private int _currIdx = 0;
    private int _prevIdx = -1;
    private int _nextIdx = -1;

    // Start is called before the first frame update
    void Start()
    {
        int idx = 0;
        // 초기 생성시 
        if (_nextIdx < 0)
        {
            foreach (StageViewer viewer in _stageViewer)
            {
                var tempViewer = Instantiate(viewer);
                tempViewer.transform.SetParent(gameObject.transform);
                tempViewer.SetStageIndex(idx++);
            }
            // Initialize general variables
            transform.localPosition = new Vector3(_widthOffset * (_stageViewer.Length / 2), 0.0f, 0.0f);
        }
    }


    private void Update()
    {

        if (_onMove != OnMove.Stop)
        {
            transform.position =
                Vector2.Lerp(new Vector2(transform.position.x, transform.position.y), new Vector2(_nextXPos, transform.position.y), _speed * Time.deltaTime);

            if (Mathf.Approximately(transform.position.x, _nextXPos))
            {
                _onMove = OnMove.Stop;
            }
        }

    }

    public void UpdateNextStage()
    {
        _nextIdx++;
    }

    public void OnMoveContentLeftButtonClicked()
    {
        // 버튼이 여러번 눌려도 움직이려는 위치까지 이동하지 않았다면 실행 X
        if (_currIdx > 0 && _onMove == OnMove.Stop)
        {
            _nextXPos = transform.position.x + _widthOffset;
            --_currIdx;
            --_prevIdx;
            _onMove = OnMove.Left;
        }
    }

    public void OnMoveContentRightButtonClicked()
    {
        if (_currIdx < _stageViewer.Length - 1 && _onMove == OnMove.Stop)
        {
            _nextXPos = transform.position.x - _widthOffset;
            ++_currIdx;
            ++_prevIdx;
            _onMove = OnMove.Right;
        }
    }
}

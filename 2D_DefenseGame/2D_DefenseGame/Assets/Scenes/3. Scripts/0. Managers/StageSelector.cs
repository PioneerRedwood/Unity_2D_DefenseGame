using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Stage Select UI
public class StageSelector : MonoBehaviour
{
    [Header("Stage")]
    [SerializeField] private StageViewer[] _stageVieweres = null;
    [SerializeField] private GameObject _content = null;
    [SerializeField] private StageDataHolder _stageDataHolder = null;
    [SerializeField] private Text _textCurrentIndex = null;

    private float _speed = 40.0f;

    private enum OnMove { Stop, Right, Left };
    private OnMove _onMove = OnMove.Stop;
    private float _nextXPos = 0;

    private int _currIdx = 0;

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
        int[] counts = Player.GetInstance().GetStageClearCount();
        
        foreach (StageViewer viewer in _stageVieweres)
        {
            var tempViewer = Instantiate(viewer);
            tempViewer.transform.SetParent(_content.transform);

            if (counts[idx] > 0)
            {
                tempViewer.InitStageViewer(StageViewer.StageViewState.Cleared);
            }
            else if(counts[idx] == 0)
            {
                tempViewer.InitStageViewer(StageViewer.StageViewState.NotTried);
            }
            else if(counts[idx] < 0)
            {
                tempViewer.InitStageViewer(StageViewer.StageViewState.Locked);
            }
            tempViewer.SetStageIndex(idx++);
        }

        _currIdx = _stageDataHolder.GetCurrentStage();
        SetContentPosition(_currIdx);

        _stageDataHolder.SetCurrentStage(_currIdx);
        _textCurrentIndex.text = _currIdx + 1 + " / " + _stageVieweres.Length;
    }

    private void SetContentPosition(int idx)
    {
        float x = 0;
        
        // 클리어 후
        if (_stageVieweres.Length % 2 == 0)
        {
            x = (Screen.width * (_stageVieweres.Length / 2) - (Screen.width / 2)) - (idx * Screen.width);
        }
        else
        {
            x = (Screen.width * (_stageVieweres.Length / 2)) - (idx * Screen.width);
        }
        _content.transform.localPosition = new Vector3(x, 0.0f, 0.0f);
    }

    #region UI Button
    public void OnMoveContentLeftButtonClicked()
    {
        // 버튼이 여러번 눌려도 움직이려는 위치까지 이동하지 않았다면 실행 X
        if ((_onMove == OnMove.Stop) && (_currIdx > 0))
        {
            _nextXPos = _content.transform.position.x + Screen.width;
            --_currIdx;
            _onMove = OnMove.Left;
            _stageDataHolder.SetCurrentStage(_currIdx);
            _textCurrentIndex.text = (_currIdx + 1) + " / " + _stageVieweres.Length;
        }
    }

    public void OnMoveContentRightButtonClicked()
    {
        if ((_onMove == OnMove.Stop) && (_currIdx < _stageVieweres.Length - 1))
        {
            _nextXPos = _content.transform.position.x - Screen.width;
            ++_currIdx;
            _onMove = OnMove.Right;
            _stageDataHolder.SetCurrentStage(_currIdx);
            _textCurrentIndex.text = (_currIdx + 1) + " / " + _stageVieweres.Length;
        }
    }

    public void OnGoToMenuButtonClicked()
    {
        SceneManager.LoadScene("StartScene");
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _UIPanel;
    [SerializeField] private GameObject[] _buttons;

    private GameObject _selectedObj;

    private int _towerPrice = 0;

    private Ground _groundComponent;
    private Route _routeComponent;

    public void RouteClick(GameObject route)
    {

        if (_selectedObj == route)
        {
            resetPanel();
            _selectedObj = null;
            return;
        }

        _selectedObj = route;
        LoadPanel(_selectedObj);

    }

    public void GroundClick(GameObject ground)
    {

        if (_selectedObj == ground)
        {
            resetPanel();
            _selectedObj = null;
            return;
        }

        _selectedObj = ground;
        LoadPanel(_selectedObj);
    }

    #region Panel

    public void LoadPanel(GameObject selectedObj)
    {
        resetPanel();

        if (selectedObj.CompareTag("Route"))
        {
            _routeComponent = selectedObj.GetComponent<Route>();

            if (_routeComponent == null)
            {
                return;
            }
        }
        else if (selectedObj.CompareTag("Ground"))
        {
            _groundComponent = selectedObj.GetComponent<Ground>();

            if (_groundComponent == null)
            {
                return;
            }

            if (_groundComponent.IsBuildTower == false)
            {
                CreateButton(0);
            }
            else if (_groundComponent.IsBuildTower == true)
            {
                CreateButton(1);
                CreateButton(2);
            }
        }
    }

    public void CreateButton(int num)
    {

        GameObject Button = Instantiate(_buttons[num]);

        Button.transform.position = _UIPanel.transform.position;
        Button.transform.SetParent(_UIPanel.transform);

        Button BtnListener = Button.GetComponent<Button>();

        switch (num)
        {
            case 0:
                BtnListener.onClick.AddListener(BuildButton);
                break;
            case 1:
                BtnListener.onClick.AddListener(MergeButton);
                break;
            case 2:
                BtnListener.onClick.AddListener(SellButton);
                break;
            default:
                break;
        }

    }

    public void resetPanel()
    {
        Transform child = _UIPanel.GetComponentInChildren<Transform>();

        foreach (Transform iter in child)
        {
            // 부모(this.gameObject)는 삭제 하지 않기 위한 처리
            if (iter != _UIPanel.transform)
            {
                Destroy(iter.gameObject);
            }
        }
    }

    #endregion

    #region GroundButtons

    public void BuildButton()
    {

        if ((Player.GetInstance().getMoney() - _towerPrice) < 0)
        {
            Debug.Log("Not Enough Minerals");
            return;
        }

        if (_groundComponent.IsBuildTower)
        {
            return;
        }

        _groundComponent.IsBuildTower = true;

        Player.GetInstance().BuildTower(_selectedObj);
        LoadPanel(_selectedObj);
    }

    public void MergeButton()
    {
        Debug.Log("MERGE");

        if (!_groundComponent.IsBuildTower)
        {
            return;
        }

        Player.GetInstance().MergeTower(_selectedObj);
    }

    public void SellButton()
    {
        Debug.Log("SELL");
        if (_groundComponent.IsBuildTower == false)
        {
            return;
        }

        Player.GetInstance().DeleteTower(_selectedObj.transform.parent);
        Player.GetInstance().addMoney(_towerPrice / 2);

        _groundComponent.IsBuildTower = false;
        LoadPanel(_selectedObj);
    }
    #endregion

}

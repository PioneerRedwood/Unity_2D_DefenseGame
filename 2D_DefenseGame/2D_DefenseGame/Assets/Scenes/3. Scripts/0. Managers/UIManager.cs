﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject _UIPanel = null;
    [SerializeField] private GameObject[] _buttons = null;
    [SerializeField] private InfoPanel _infoPanel = null;
    [SerializeField] private TowerAttackRange _towerAttackRange = null;
    [SerializeField] private GameObject _towerUpgradePanel = null;
    private bool _isTowerUpgradePanelOpen = false;

    [Header("Tower")]
    [SerializeField] private int _towerPrice = 0;

    [Header("Obstacle")]
    [SerializeField] private int _obstaclePrice = 0;

    private GameObject _selectedObj;
    private Ground _groundComponent;
    private Route _routeComponent;

    private void Start()
    {
        InvokeRepeating("UpdateInfoPanel", 0.0f, 0.2f);
    }

    #region Object Clicked

    public void RouteClicked(GameObject route)
    {
        if (_selectedObj == route)
        {
            ResetPanel();
            _selectedObj = null;
            return;
        }

        _selectedObj = route;
        LoadPanel(_selectedObj);
    }

    public void GroundClicked(GameObject ground)
    {
        if (_selectedObj == ground)
        {
            ResetPanel();
            _selectedObj = null;
            return;
        }

        _selectedObj = ground;
        LoadPanel(_selectedObj);
    }

    #endregion

    #region Main panel

    private void UpdateInfoPanel()
    {
        if (_selectedObj == null)
        {
            ResetPanel();
            return;
        }

        if (_selectedObj.CompareTag("Ground"))
        {
            if ((Player.GetInstance().GetTower(_selectedObj.transform.parent) != null)
                && (Player.GetInstance().GetTower(_selectedObj.transform.parent).CompareTag("Tower")))
            {
                LoadTowerInfo();
            }
        }
        else if (_selectedObj.CompareTag("Enemy"))
        {
            LoadMonsterInfo();
        }
        else if (_selectedObj.CompareTag("Route"))
        {
            if(_selectedObj.GetComponent<Route>().GetObstacleBuilt())
            {
                LoadObstacleInfo();
            }
        }
    }

    public void LoadPanel(GameObject selectedObj)
    {
        ResetPanel();

        if (selectedObj.CompareTag("Route"))
        {
            _routeComponent = selectedObj.GetComponent<Route>();

            if (_routeComponent == null)
            {
                return;
            }

            if (_routeComponent.GetObstacleBuilt())
            {
                LoadObstacleInfo();
            }
            else
            {
                CreateButton(3);
            }
        }
        else if (selectedObj.CompareTag("Ground"))
        {
            _groundComponent = selectedObj.GetComponent<Ground>();

            if (_groundComponent == null)
            {
                return;
            }

            if (_groundComponent.GetTowerBuilt())
            {
                CreateButton(1);
                CreateButton(2);
                LoadTowerInfo();
            }
            else
            {
                CreateButton(0);
            }
        }
        else if (selectedObj.CompareTag("Enemy"))
        {
            LoadMonsterInfo();
        }
    }

    public void CreateButton(int num)
    {
        GameObject button = Instantiate(_buttons[num]);

        button.transform.position = _UIPanel.transform.position;
        button.transform.SetParent(_UIPanel.transform);

        Button btnListener = button.GetComponent<Button>();

        switch (num)
        {
            case 0:
                btnListener.onClick.AddListener(BuildTowerOnGround);
                break;
            case 1:
                btnListener.onClick.AddListener(MergeTower);
                break;
            case 2:
                btnListener.onClick.AddListener(SellTower);
                break;
            case 3:
                btnListener.onClick.AddListener(InstallObstacleOnRoute);
                break;
            default:
                break;
        }

    }
    
    public void ResetPanel()
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

        _infoPanel.OffPanel();
        _towerAttackRange.OffAttackRange();
    }

    #endregion

    #region GroundButtons

    //건설 버튼
    private void BuildTowerOnGround()
    {
        if (_groundComponent.GetTowerBuilt())
        {
            return;
        }

        if ((Player.GetInstance().GetMoney() - _towerPrice) < 0)
        {
            Player.GetInstance().ShowAlert("Not enough money");
            return;
        }
        else
        {
            Player.GetInstance().LoseMoney(_towerPrice);
            Player.GetInstance().ShowAlert("Tower build completed");
        }

        _groundComponent.SetTowerBuilt(true);

        Player.GetInstance().BuildTower(_selectedObj);
        LoadPanel(_selectedObj);
    }

    private void MergeTower()
    {
        if (!_groundComponent.GetTowerBuilt())
        {
            return;
        }

        if (Player.GetInstance().MergeTower(_selectedObj))
        {
            Player.GetInstance().ShowAlert("Tower merge completed");
        }
        else
        {
            Player.GetInstance().ShowAlert("There is no proper tower to merge");
        }

        LoadPanel(_selectedObj);
    }

    private void SellTower()
    {
        int sellPrice = 0;
        if (!_groundComponent.GetTowerBuilt())
        {
            return;
        }
        switch (Player.GetInstance().GetTower(_selectedObj.transform.parent)._tier)
        {
            case Tower.TowerTier.Common:
                sellPrice = _towerPrice / 2;
                break;
            case Tower.TowerTier.Uncommon:
                sellPrice = _towerPrice;
                break;
            case Tower.TowerTier.Rare:
                sellPrice = _towerPrice * 2;
                break;
            case Tower.TowerTier.Unique:
                sellPrice = _towerPrice * 4;
                break;
            case Tower.TowerTier.Legendary:
                sellPrice = _towerPrice * 8;
                break;
            default:
                break;
        }

        Player.GetInstance().DeleteTower(_selectedObj.transform.parent);
        Player.GetInstance().AddMoney(sellPrice);

        Player.GetInstance().ShowAlert("Sell tower +$" + sellPrice);
        _groundComponent.SetTowerBuilt(false);
        LoadPanel(_selectedObj);
    }
    #endregion

    #region GroundButton TowerInfo

    private void LoadTowerInfo()
    {
        Tower selectedTower = Player.GetInstance().GetTower(_selectedObj.transform.parent);
        if (selectedTower != null)
        {
            _infoPanel.OnTowerPanel(selectedTower);
            _towerAttackRange.OnAttackRange(selectedTower._range, selectedTower.transform.position);
        }
    }

    #endregion

    #region Route

    private void LoadObstacleInfo()
    {
        Obstacle selectedObstacle = _selectedObj.GetComponentInChildren<Obstacle>();

        if (selectedObstacle != null)
        {
            _infoPanel.OnObstaclePanel(selectedObstacle);
        }
    }

    private void InstallObstacleOnRoute()
    {
        if (_routeComponent.GetObstacleBuilt())
        {
            return;
        }

        if ((Player.GetInstance().GetMoney() - _obstaclePrice) < 0)
        {
            Player.GetInstance().ShowAlert("Not enough money");
            return;
        }
        else
        {
            Player.GetInstance().LoseMoney(_obstaclePrice);
            Player.GetInstance().ShowAlert("Obstacle install completed");
        }

        _routeComponent.SetObstacleBuilt(true);

        Player.GetInstance().InstallObstacle(_routeComponent);
        LoadPanel(_selectedObj);
    }

    #endregion

    #region Tower upgrade panel

    public void ResetTowerUpgradePanel()
    {
        GameObject.Find("CommonUpText").GetComponent<Text>().text = "Common Up \n#" + 1 + " Cost: " + 50;
        GameObject.Find("UncommonUpText").GetComponent<Text>().text = "Uncommon Up \n#" + 1 + " Cost: " + 100;
        GameObject.Find("RareUpText").GetComponent<Text>().text = "Rare Up \n#" + 1 + " Cost: " + 300;
        GameObject.Find("UniqueUpText").GetComponent<Text>().text = "Unique Up \n#" + 1 + " Cost: " + 500;
        GameObject.Find("LegendaryUpText").GetComponent<Text>().text = "Legendary Up \n#" + 1 + " Cost: " + 1000;
    }

    public void OnTowerUpgradePanelButtonClicked()
    {
        if (!_isTowerUpgradePanelOpen)
        {
            _towerUpgradePanel.GetComponent<Animator>().SetBool("Clicked", true);
            _isTowerUpgradePanelOpen = true;

            Sprite rightArrow = Resources.Load<Sprite>("right arrow");
            _towerUpgradePanel.GetComponentInChildren<Image>().sprite = rightArrow;

        }
        else
        {
            _towerUpgradePanel.GetComponent<Animator>().SetBool("Clicked", false);
            _isTowerUpgradePanelOpen = false;

            Sprite leftArrow = Resources.Load<Sprite>("left arrow");
            _towerUpgradePanel.GetComponentInChildren<Image>().sprite = leftArrow;
        }
    }

    #endregion

    #region Monster selected

    public void LoadMonsterInfo()
    {
        Monster[] totalMonsters = FindObjectsOfType<Monster>();

        foreach (Monster monster in totalMonsters)
        {
            if (monster == null)
            {
                continue;
            }

            if (_selectedObj.GetComponent<Monster>().Equals(monster))
            {
                _infoPanel.OnMonsterPanel(monster);
                return;
            }
        }
    }

    public void MonsterClicked(GameObject monster)
    {
        if (_selectedObj == monster)
        {
            ResetPanel();
            _selectedObj = null;
            return;
        }

        _selectedObj = monster;
        LoadPanel(_selectedObj);
    }

    #endregion
}

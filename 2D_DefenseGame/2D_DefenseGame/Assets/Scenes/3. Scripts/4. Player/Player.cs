using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private static Player _instance;
    public static Player GetInstance()
    {
        if (_instance == null)
        {
            _instance = new Player();
        }

        return _instance;
    }

    private TowerManager _towerManager;

    [Header("Player property")]
    [SerializeField] private int _money = 0;
    [SerializeField] private int _life = 0;

    private int _currMoney;
    private int _currLife;

    [Header("Tower tier cost")]
    [SerializeField] private uint _commonUpCost = 0;
    [SerializeField] private uint _uncommonUpCost = 0;
    [SerializeField] private uint _rareUpCost = 0;
    [SerializeField] private uint _uniqueUpCost = 0;
    [SerializeField] private uint _legendaryUpCost = 0;

    [Header("Tower tier up damage")]
    [SerializeField] private uint _commonDamageUp = 0;
    [SerializeField] private uint _uncommonDamageUp = 0;
    [SerializeField] private uint _rareDamageUp = 0;
    [SerializeField] private uint _uniqueDamageUp = 0;
    [SerializeField] private uint _legendaryDamageUp = 0;

    private uint _commonLevel = 1;
    private uint _uncommonLevel = 1;
    private uint _rareLevel = 1;
    private uint _uniqueLevel = 1;
    private uint _legendaryLevel = 1;

    private List<Tower> _towerList = new List<Tower>();

    private bool _isAlertOpen = false;
    private Text alertText = null;
    private float _gameSpeedScale = 1.0f;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    private void Start()
    {
        _currMoney = _money;
        _currLife = _life;
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("GameScene"))
        {
            alertText = GameObject.Find("AlertText").GetComponent<Text>();
            alertText.CrossFadeAlpha(0f, 0f, true);
        }
    }

    public float GetGameSpeed()
    {
        return _gameSpeedScale;
    }

    public void UpGameSpeed()
    {
        if(_gameSpeedScale <= 1.5f)
        {
            _gameSpeedScale += 0.1f;
            Time.timeScale = _gameSpeedScale;
            ShowAlert("Game speed up");
        }
        else
        {
            _gameSpeedScale = 1.0f;
            Time.timeScale = _gameSpeedScale;
            ShowAlert("Game speed Range: 0.5 ~ 1.5");
        }
    }

    public void DownGameSpeed()
    {
        if(_gameSpeedScale >= 0.5f)
        {
            _gameSpeedScale -= 0.1f;
            Time.timeScale = _gameSpeedScale;
            ShowAlert("Game speed down");
        }
        else
        {
            _gameSpeedScale = 1.0f;
            Time.timeScale = _gameSpeedScale;
            ShowAlert("Game speed Range: 0.5 ~ 1.5");
        }
    }


    #region Player basic method
    public void ResetGame()
    {
        _currMoney = _money;
        _currLife = _life;
        _towerList.Clear();
    }
    public void AddMoney(int num)
    {
        _currMoney += num;
    }
    public int GetMoney()
    {
        return _currMoney;
    }
    public void LoseMoney(int num)
    {
        _currMoney -= num;
    }
    public void AddLife(int num)
    {
        _currLife += num;
    }
    public int GetLife()
    {
        return _currLife;
    }
    public void LoseLife(int num)
    {
        _currLife -= num;
    }

    public void ShowAlert(string msg)
    {
        if (!_isAlertOpen)
        {
            alertText.text = msg;
            alertText.CrossFadeAlpha(1f, 0.5f, true);
            _isAlertOpen = true;
            StartCoroutine(DelayAlert());
        }
    }

    private IEnumerator DelayAlert()
    {
        yield return new WaitForSeconds(1f);
        alertText.CrossFadeAlpha(0f, 0.5f, true);
        _isAlertOpen = false;
    }

    #endregion

    #region Handle Tower

    public List<Tower> GetTowerList()
    {
        return _towerList;
    }

    public Tower GetTower(Transform parent)
    {
        Tower find = null;
        foreach (Tower tower in _towerList)
        {
            if (tower.transform.parent == parent)
            {
                find = tower;
                break;
            }
        }
        return find;
    }

    public void BuildTower(GameObject selectedObj)
    {
        Tower tempTower = Instantiate(GetRandomTower(_towerManager.Common), selectedObj.transform.position, Quaternion.identity);

        tempTower._defaultDamage += _commonDamageUp * (_commonLevel - 1);

        tempTower.transform.position = selectedObj.transform.position;
        tempTower.transform.SetParent(selectedObj.transform.parent);

        MissionManager.GetInstance()._commonCount += 1;
        AddTower(tempTower);
    }

    public bool BuildTower(GameObject selectedObj, Tower tower)
    {
        Tower tempTower = Instantiate(tower, selectedObj.transform.position, Quaternion.identity);

        switch (tempTower._tier)
        {
            case Tower.TowerTier.Uncommon:
                tempTower._defaultDamage += _uncommonDamageUp * (_uncommonLevel - 1);
                break;
            case Tower.TowerTier.Rare:
                tempTower._defaultDamage += _rareDamageUp * (_rareLevel - 1);
                break;
            case Tower.TowerTier.Unique:
                tempTower._defaultDamage += _uniqueDamageUp * (_uniqueLevel - 1);
                break;
            case Tower.TowerTier.Legendary:
                tempTower._defaultDamage += _legendaryDamageUp * (_legendaryLevel - 1);
                break;
            default:
                break;
        }
        tempTower.transform.position = selectedObj.transform.position;
        tempTower.transform.SetParent(selectedObj.transform.parent);

        AddTower(tempTower);
        if (tempTower != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DeleteTower(Transform delete)
    {
        int towerindex = 0;

        foreach (Tower tower in _towerList)
        {

            if (tower.transform.parent == delete)
            {
                _towerList.RemoveAt(towerindex);
                Destroy(tower.gameObject);
                break;
            }

            towerindex++;
        }
    }

    public bool MergeTower(GameObject selectedObj)
    {
        bool isCompleted = false;
        Tower selectedTower = GetTower(selectedObj.transform.parent);

        foreach (Tower tower in _towerList)
        {
            if ((tower != selectedTower) && tower._towerName.Equals(selectedTower._towerName) && (tower._tier.Equals(selectedTower._tier)))
            {
                switch (selectedTower._tier)
                {
                    case Tower.TowerTier.Common:
                        isCompleted = BuildTower(selectedObj, GetRandomTower(_towerManager.Uncommon));
                        MissionManager.GetInstance()._uncommonCount += 1;
                        break;
                    case Tower.TowerTier.Uncommon:
                        isCompleted = BuildTower(selectedObj, GetRandomTower(_towerManager.Rare));
                        MissionManager.GetInstance()._rareCount += 1;
                        break;
                    case Tower.TowerTier.Rare:
                        isCompleted = BuildTower(selectedObj, GetRandomTower(_towerManager.Unique));
                        MissionManager.GetInstance()._uniqueCount += 1;
                        break;
                    case Tower.TowerTier.Unique:
                        isCompleted = BuildTower(selectedObj, GetRandomTower(_towerManager.Legendary));
                        MissionManager.GetInstance()._legendaryCount += 1;
                        break;
                    case Tower.TowerTier.Legendary:
                        break;
                    default:
                        break;
                }
                
                _towerList.Remove(selectedTower);
                _towerList.Remove(tower);

                tower.transform.parent.GetChild(0).GetComponent<Ground>().SetTowerBuilt(false);

                Destroy(selectedTower.gameObject);
                Destroy(tower.gameObject);
                break;
            }
        }
        return isCompleted;
    }

    public void AddTower(Tower tower)
    {
        if (tower == null)
        {
            return;
        }

        _towerList.Add(tower);
    }

    public void LoadTower(TowerManager towerManager)
    {
        _towerManager = towerManager;
        if (_towerManager == null)
        {
            _towerManager = GameObject.FindObjectOfType<TowerManager>();
        }
    }

    private Tower GetRandomTower(Tower[] List)
    {
        Tower randomTower = List[Random.Range(0, List.Length)];

        return randomTower;
    }

    #endregion

    #region Upgrade tower
    private void UpgradeCommonTower()
    {
        if (_currMoney >= (int)(_commonUpCost * _commonLevel))
        {
            foreach (Tower tower in GetInstance().GetTowerList())
            {
                if (tower._tier == Tower.TowerTier.Common)
                {
                    tower._defaultDamage += _commonDamageUp * _commonLevel;
                }
            }
            _currMoney -= (int)(_commonUpCost * _commonLevel++);
            GameObject.Find("CommonUpText").GetComponent<Text>().text = "Common Up \n#" + _commonLevel + " Cost: " + (_commonUpCost * _commonLevel);

            ShowAlert("Upgrade common tower completed");
        }
        else
        {
            ShowAlert("Not enough money");
        }
    }

    private void UpgradeUncommonTower()
    {
        if (_currMoney >= (int)(_uncommonUpCost * _uncommonLevel))
        {
            foreach (Tower tower in GetInstance().GetTowerList())
            {
                if (tower._tier == Tower.TowerTier.Uncommon)
                {
                    tower._defaultDamage += _uncommonDamageUp * _uncommonLevel;
                }
            }
            _currMoney -= (int)(_uncommonUpCost * _uncommonLevel++);
            GameObject.Find("UncommonUpText").GetComponent<Text>().text = "Uncommon Up \n#" + _uncommonLevel + " Cost: " + (_uncommonUpCost * _uncommonLevel);
            ShowAlert("Upgrade uncommon tower completed");
        }
        else
        {
            ShowAlert("Not enough money");
        }
    }

    private void UpgradeRareTower()
    {
        if (_currMoney >= (int)(_rareUpCost * _rareLevel))
        {
            foreach (Tower tower in GetInstance().GetTowerList())
            {
                if (tower._tier == Tower.TowerTier.Rare)
                {
                    tower._defaultDamage += _rareDamageUp * _rareLevel;
                }
            }
            _currMoney -= (int)(_rareUpCost * _rareLevel++);
            GameObject.Find("RareUpText").GetComponent<Text>().text = "Rare Up \n#" + _rareLevel + " Cost: " + (_rareUpCost * _rareLevel);
            ShowAlert("Upgrade rare tower completed");
        }
        else
        {
            ShowAlert("Not enough money");
        }
    }

    private void UpgradeUniqueTower()
    {
        Tower[] towerList = FindObjectsOfType<Tower>();

        if (_currMoney >= (int)(_uniqueUpCost * _uniqueLevel))
        {
            foreach (Tower tower in GetInstance().GetTowerList())
            {
                if (tower._tier == Tower.TowerTier.Unique)
                {
                    tower._defaultDamage += _uniqueDamageUp * _uniqueLevel;
                }
            }
            _currMoney -= (int)(_uniqueUpCost * _uniqueLevel++);
            GameObject.Find("UniqueUpText").GetComponent<Text>().text = "Unique Up \n#" + _uniqueLevel + " Cost: " + (_uniqueUpCost * _uniqueLevel);
            ShowAlert("Upgrade unique tower completed");
        }
        else
        {
            ShowAlert("Not enough money");
        }
    }

    private void UpgradeLegendaryTower()
    {
        if (_currMoney >= (int)(_legendaryUpCost * _legendaryLevel))
        {
            foreach (Tower tower in GetInstance().GetTowerList())
            {
                if (tower._tier == Tower.TowerTier.Legendary)
                {
                    tower._defaultDamage += _legendaryDamageUp * _legendaryLevel;
                }
            }
            _currMoney -= (int)(_legendaryUpCost * _legendaryLevel++);
            GameObject.Find("LegendaryUpText").GetComponent<Text>().text = "Legendary Up \n#" + _legendaryLevel + " Cost: " + (_legendaryUpCost * _legendaryLevel);
            ShowAlert("Upgrade legendary tower completed");
        }
        else
        {
            ShowAlert("Not enough money");
        }
    }
    #endregion

    #region Handle Obstacle

    public void InstallObstacle(Route route)
    {
        Obstacle obstacle = Instantiate(GamePrefabManager.Instance.obstaclePrefab);

        obstacle.InitObstacleOnRoute(route);
        obstacle.SetName(obstacle.GetName() + " " + (Vector2)route.transform.position);
        obstacle.transform.position = route.transform.position;
        obstacle.transform.SetParent(route.transform);
    }

    #endregion
}

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

    [HideInInspector] public int _numOfStage = 0;

    private int _currMoney;
    private int _currLife;

    #region Tower property

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

    #endregion

    private bool _isAlertOpen = false;
    private Text _alertText = null;

    private bool _toggleGameSpeed = false;
    private float _gameSpeedScale = 1.0f;
    private int[] _clearCounts = null;
    private System.Text.StringBuilder _gameLog;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            _numOfStage = PlayerPrefs.GetString("StageClearCount").Split('\t').Length;
            _clearCounts = new int[_numOfStage];
            _clearCounts = GetStageClearCount();
            _gameLog = new System.Text.StringBuilder();
        }
    }

    private void Start()
    {
        _currMoney = _money;
        _currLife = _life;
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("GameScene"))
        {
            _alertText = GameObject.Find("AlertText").GetComponent<Text>();
            _alertText.CrossFadeAlpha(0f, 0f, true);
        }
    }

    #region Game play :: Stage clear state

    public void AddStageClearCount(int stageIdx)
    {
        if (stageIdx < _numOfStage)
        {
            _clearCounts[stageIdx] += 1;
            if ((stageIdx + 1) != _numOfStage)
            {
                _clearCounts[stageIdx + 1] = 0;
            }
        }

        for (int i = 0; i < _numOfStage; i++)
        {
            _gameLog.Append(i + 1);
            _gameLog.Append(":");
            _gameLog.Append(_clearCounts[i]);
            if ((i + 1) != _numOfStage)
            {
                _gameLog.Append('\t');
            }
        }

        PlayerPrefs.SetString("StageClearCount", _gameLog.ToString());
        PlayerPrefs.Save();
    }

    public int[] GetStageClearCount()
    {
        int idx = 0;
        string[] strs = PlayerPrefs.GetString("StageClearCount").Split('\t');

        foreach (string str in strs)
        {
            string[] tempStrs = str.Split(':');
            _clearCounts[idx] = int.Parse(tempStrs[1]);
            idx++;
        }

        return _clearCounts;
    }

    #endregion

    #region Handle Game Speed

    public void ToggleGameSpeed()
    {
        if (_toggleGameSpeed)
        {
            _gameSpeedScale /= 2.0f;
            _toggleGameSpeed = false;
        }
        else
        {
            _gameSpeedScale *= 2.0f;
            _toggleGameSpeed = true;
        }

        Time.timeScale = _gameSpeedScale;
        ShowAlert("Toggle game speed");
    }

    #endregion

    #region Player basic method

    public void ResetGame()
    {
        _currMoney = _money;
        _currLife = _life;

        _commonLevel = 1;
        _uncommonLevel = 1;
        _rareLevel = 1;
        _uniqueLevel = 1;
        _legendaryLevel = 1;

        GameObject.Find("UIManager").GetComponent<UIManager>().ResetTowerUpgradePanel();

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
            _alertText.text = msg;
            _alertText.CrossFadeAlpha(1f, 1f, true);
            _isAlertOpen = true;
            StartCoroutine(DelayAlert());
        }
    }

    private IEnumerator DelayAlert()
    {
        yield return new WaitForSeconds(2f);
        _alertText.CrossFadeAlpha(0f, 0.5f, true);
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

    public bool BuildTower(GameObject selectedObj)
    {
        Tower tempTower = Instantiate(GetRandomTower(_towerManager.Common), selectedObj.transform.position, Quaternion.identity);

        tempTower._defaultDamage += _commonDamageUp * (_commonLevel - 1);
        tempTower._range += 0.1f * (_commonLevel - 1);

        tempTower.transform.position = selectedObj.transform.position;
        tempTower.transform.SetParent(selectedObj.transform.parent);

        if (tempTower != null)
        {
            MissionManager.GetInstance().UpdateLedger(tempTower._tier, tempTower._towerName, 1);
            AddTower(tempTower);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool BuildTower(GameObject selectedObj, Tower tower)
    {
        Tower tempTower = Instantiate(tower, selectedObj.transform.position, Quaternion.identity);

        switch (tempTower._tier)
        {
            case Tower.TowerTier.Uncommon:
                tempTower._defaultDamage += _uncommonDamageUp * (_uncommonLevel - 1);
                tempTower._range += 0.1f * (_uncommonLevel - 1);
                break;
            case Tower.TowerTier.Rare:
                tempTower._defaultDamage += _rareDamageUp * (_rareLevel - 1);
                tempTower._range += 0.1f * (_rareLevel - 1);
                break;
            case Tower.TowerTier.Unique:
                tempTower._defaultDamage += _uniqueDamageUp * (_uniqueLevel - 1);
                tempTower._range += 0.1f * (_uniqueLevel - 1);
                break;
            case Tower.TowerTier.Legendary:
                tempTower._defaultDamage += _legendaryDamageUp * (_legendaryLevel - 1);
                tempTower._range += 0.1f * (_legendaryLevel - 1);
                break;
            default:
                break;
        }
        tempTower.transform.position = selectedObj.transform.position;
        tempTower.transform.SetParent(selectedObj.transform.parent);


        if (tempTower != null)
        {
            MissionManager.GetInstance().UpdateLedger(tempTower._tier, tempTower._towerName, 1);
            AddTower(tempTower);
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
                _towerList.Remove(tower);
                MissionManager.GetInstance().UpdateLedger(tower._tier, tower._towerName, -1);
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
        Debug.Log(_towerList.Count);

        foreach (Tower tower in _towerList)
        {
            if ((tower != selectedTower) && tower._towerName.Equals(selectedTower._towerName) && (tower._tier.Equals(selectedTower._tier)))
            {
                switch (selectedTower._tier)
                {
                    case Tower.TowerTier.Common:
                        isCompleted = BuildTower(selectedObj, GetRandomTower(_towerManager.Uncommon));
                        break;
                    case Tower.TowerTier.Uncommon:
                        isCompleted = BuildTower(selectedObj, GetRandomTower(_towerManager.Rare));
                        break;
                    case Tower.TowerTier.Rare:
                        isCompleted = BuildTower(selectedObj, GetRandomTower(_towerManager.Unique));
                        break;
                    case Tower.TowerTier.Unique:
                        isCompleted = BuildTower(selectedObj, GetRandomTower(_towerManager.Legendary));
                        break;
                    case Tower.TowerTier.Legendary:
                        ShowAlert("There is no next from Legedary");
                        isCompleted = true;
                        break;
                    default:
                        break;
                }

                if (isCompleted)
                {
                    MissionManager.GetInstance().UpdateLedger(selectedTower._tier, selectedTower._towerName, -1);
                    MissionManager.GetInstance().UpdateLedger(tower._tier, tower._towerName, -1);

                    if (_towerList.Remove(selectedTower) && _towerList.Remove(tower))
                    {
                        tower.transform.parent.GetChild(0).GetComponent<Ground>().SetTowerBuilt(false);

                        Destroy(selectedTower.gameObject);
                        Destroy(tower.gameObject);
                    }
                }
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

    public void UpgradeTower(string idx)
    {
        switch (int.Parse(idx))
        {
            case 0:
                if (_currMoney >= (int)(_commonUpCost * _commonLevel))
                {
                    foreach (Tower tower in GetInstance().GetTowerList())
                    {
                        if (tower._tier == Tower.TowerTier.Common)
                        {
                            tower._defaultDamage += _commonDamageUp * _commonLevel;
                            tower._range += 0.1f;
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

                break;
            case 1:
                if (_currMoney >= (int)(_uncommonUpCost * _uncommonLevel))
                {
                    foreach (Tower tower in GetInstance().GetTowerList())
                    {
                        if (tower._tier == Tower.TowerTier.Uncommon)
                        {
                            tower._defaultDamage += _uncommonDamageUp * _uncommonLevel;
                            tower._range += 0.1f;
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
                break;
            case 2:
                if (_currMoney >= (int)(_rareUpCost * _rareLevel))
                {
                    foreach (Tower tower in GetInstance().GetTowerList())
                    {
                        if (tower._tier == Tower.TowerTier.Rare)
                        {
                            tower._defaultDamage += _rareDamageUp * _rareLevel;
                            tower._range += 0.1f;
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
                break;
            case 3:
                if (_currMoney >= (int)(_uniqueUpCost * _uniqueLevel))
                {
                    foreach (Tower tower in GetInstance().GetTowerList())
                    {
                        if (tower._tier == Tower.TowerTier.Unique)
                        {
                            tower._defaultDamage += _uniqueDamageUp * _uniqueLevel;
                            tower._range += 0.1f;
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
                break;
            case 4:
                if (_currMoney >= (int)(_legendaryUpCost * _legendaryLevel))
                {
                    foreach (Tower tower in GetInstance().GetTowerList())
                    {
                        if (tower._tier == Tower.TowerTier.Legendary)
                        {
                            tower._defaultDamage += _legendaryDamageUp * _legendaryLevel;
                            tower._range += 0.1f;
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
                break;
            default:
                break;
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

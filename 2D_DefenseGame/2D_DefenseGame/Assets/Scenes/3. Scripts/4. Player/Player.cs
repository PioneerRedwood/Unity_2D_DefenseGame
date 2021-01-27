using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    #region Player basic method
    public void ResetGame()
    {
        _money = 0;
        _life = 0;
        _towerList.Clear();
    }
    public void AddMoney(int num)
    {
        _money += num;
    }
    public int GetMoney()
    {
        return _money;
    }
    public void LoseMoney(int num)
    {
        _money -= num;
    }
    public void AddLife(int num)
    {
        _life += num;
    }
    public int GetLife()
    {
        return _life;
    }
    public void LoseLife(int num)
    {
        _life -= num;
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

        tempTower.transform.position = selectedObj.transform.position;
        tempTower.transform.SetParent(selectedObj.transform.parent);

        AddTower(tempTower);
    }

    public void BuildTower(GameObject selectedObj, Tower tower)
    {
        Tower _tempTower = Instantiate(tower, selectedObj.transform.position, Quaternion.identity);

        _tempTower.transform.position = selectedObj.transform.position;
        _tempTower.transform.SetParent(selectedObj.transform.parent);

        AddTower(_tempTower);
    }

    public void DeleteTower(Transform delete)
    {
        int towerindex = 0;

        foreach (Tower tower in _towerList)
        {

            if (tower.transform.parent == delete)
            {
                Debug.Log("Deleted!");

                _towerList.RemoveAt(towerindex);
                Destroy(tower.gameObject);
                break;
            }

            towerindex++;
        }
    }

    public void MergeTower(GameObject selectedObj)
    {
        Tower SelectedTower = GetTower(selectedObj.transform.parent);

        foreach (Tower tower in _towerList)
        {
            if (tower._towerName == SelectedTower._towerName && tower != SelectedTower)
            {
                switch (SelectedTower._tier.ToString())
                {
                    case "Common":
                        BuildTower(selectedObj, GetRandomTower(_towerManager.Uncommon));
                        break;
                    case "Uncommon":
                        BuildTower(selectedObj, GetRandomTower(_towerManager.Rare));
                        break;
                    case "Rare":
                        BuildTower(selectedObj, GetRandomTower(_towerManager.Unique));
                        break;
                    case "Unique":
                        BuildTower(selectedObj, GetRandomTower(_towerManager.Legendary));
                        break;
                    case "Legendary":
                        break;
                    default:
                        break;
                }

                _towerList.Remove(SelectedTower);
                _towerList.Remove(tower);

                tower.transform.parent.GetChild(0).GetComponent<Ground>().IsBuildTower = false;

                Destroy(SelectedTower.gameObject);
                Destroy(tower.gameObject);

                break;
            }
        }
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

    public void UpgradeCommonTower()
    {
        // _towerList에 기존에 씬에 둔 타워가 저장이 되지 않아 임의로 배열 만들어서 넣어둠
        Tower[] towerList = FindObjectsOfType<Tower>();
        Debug.Log("Up Common Tower " + towerList.Length);

        if (_money >= (int)(_commonUpCost * _commonLevel))
        {
            foreach (Tower tower in towerList)
            {
                if (tower._tier == Tower.TowerTier.Common)
                {
                    tower._defaultDamage += _commonDamageUp * _commonLevel;
                }
            }
            _commonLevel++;
            _money -= (int)(_commonUpCost * _commonLevel);
            GameObject.Find("CommonUpText").GetComponent<Text>().text = "Common Up \nCost: " + (_commonUpCost * _commonLevel);
        }
    }

    public void UpgradeUncommonTower()
    {
        Tower[] towerList = FindObjectsOfType<Tower>();
        Debug.Log("Up Uncommon Tower " + towerList.Length);

        if (_money >= (int)(_uncommonUpCost * _uncommonLevel))
        {
            foreach (Tower tower in towerList)
            {
                if (tower._tier == Tower.TowerTier.Uncommon)
                {
                    tower._defaultDamage += _uncommonDamageUp * _uncommonLevel;
                }
            }
            _uncommonLevel++;
            _money -= (int)(_uncommonUpCost * _uncommonLevel);
            GameObject.Find("UncommonUpText").GetComponent<Text>().text = "Uncommon Up \nCost: " + (_uncommonUpCost * _uncommonLevel);
        }
    }

    public void UpgradeRareTower()
    {
        Tower[] towerList = FindObjectsOfType<Tower>();
        Debug.Log("Up Rare Tower " + towerList.Length);

        if (_money >= (int)(_rareUpCost * _rareLevel))
        {
            foreach (Tower tower in towerList)
            {
                if (tower._tier == Tower.TowerTier.Rare)
                {
                    tower._defaultDamage += _rareDamageUp * _rareLevel;
                }
            }
            _rareLevel++;
            _money -= (int)(_rareUpCost * _rareLevel);
            GameObject.Find("RareUpText").GetComponent<Text>().text = "Rare Up \nCost: " + (_rareUpCost * _rareLevel);
        }
    }

    public void UpgradeUniqueTower()
    {
        Tower[] towerList = FindObjectsOfType<Tower>();
        Debug.Log("Up Unique Tower " + towerList.Length);

        if (_money >= (int)(_uniqueUpCost * _uniqueLevel))
        {
            foreach (Tower tower in towerList)
            {
                if (tower._tier == Tower.TowerTier.Unique)
                {
                    tower._defaultDamage += _uniqueDamageUp * _uniqueLevel;
                }
            }
            _uniqueLevel++;
            _money -= (int)(_uniqueUpCost * _uniqueLevel);
            GameObject.Find("UniqueUpText").GetComponent<Text>().text = "Unique Up \nCost: " + (_uniqueUpCost * _uniqueLevel);
        }
    }

    public void UpgradeLegendaryTower()
    {
        Tower[] towerList = FindObjectsOfType<Tower>();
        Debug.Log("Up Legendary Tower " + towerList.Length);

        if (_money >= (int)(_legendaryUpCost * _legendaryLevel))
        {
            foreach (Tower tower in towerList)
            {
                if (tower._tier == Tower.TowerTier.Legendary)
                {
                    tower._defaultDamage += _legendaryDamageUp * _legendaryLevel;
                }
            }
            _legendaryLevel++;
            _money -= (int)(_legendaryUpCost * _legendaryLevel);
            GameObject.Find("LegendaryUpText").GetComponent<Text>().text = "Legendary Up \nCost: " + (_legendaryUpCost * _legendaryLevel);
        }
    }
    #endregion

}

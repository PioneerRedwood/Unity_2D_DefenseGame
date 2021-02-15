using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    #region Singleton Manager

    private static MissionManager _instance;

    public static MissionManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new MissionManager();
        }

        return _instance;
    }

    #endregion

    [SerializeField] private GameObject _missionPanel = null;
    [SerializeField] private Text _towerTierLedgerText = null;
    [SerializeField] private Text _towerTypeLedgerText = null;
    [SerializeField] private GameObject _missionBtnPref = null;

    private List<MissionHolder> _holders = null;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    #region Mission UI

    private void InitMissionHolder()
    {
        int idx = 0;

        foreach (Mission mission in _missions)
        {
            if (!mission._isCleared)
            {
                GameObject btnPref = Instantiate(_missionBtnPref);

                btnPref.transform.position = _missionPanel.transform.position;
                btnPref.transform.SetParent(_missionPanel.transform);

                MissionHolder holder = btnPref.GetComponent<MissionHolder>();
                btnPref.GetComponentInChildren<Text>().text = "Mission #" + (++idx).ToString();
                holder.SetMissionInfo(btnPref.GetComponentInChildren<Text>().text, mission);
                _holders.Add(holder);
            }
        }
    }

    #endregion

    #region Tower Ledger

    [System.Serializable]
    public struct TowerLedger
    {
        public uint _numOfCommon;
        public uint _numOfUncommon;
        public uint _numOfRare;
        public uint _numOfUnique;
        public uint _numOfLegendary;

        public uint _numOfBullet;
        public uint _numOfAOE;
        public uint _numOfBuff;
        public uint _numOfSlow;
        public uint _numOfLaser;

        public TowerLedger(uint un)
        {
            _numOfCommon = un;
            _numOfUncommon = un;
            _numOfRare = un;
            _numOfUnique = un;
            _numOfLegendary = un;

            _numOfBullet = un;
            _numOfAOE = un;
            _numOfBuff = un;
            _numOfSlow = un;
            _numOfLaser = un;
        }
    }

    private TowerLedger _towerLedger = new TowerLedger(0);

    public TowerLedger GetTowerLedger()
    {
        return _towerLedger;
    }

    public void InitLedger()
    {
        _towerLedger._numOfCommon = 0;
        _towerLedger._numOfUncommon = 0;
        _towerLedger._numOfRare = 0;
        _towerLedger._numOfUnique = 0;
        _towerLedger._numOfLegendary = 0;

        _towerLedger._numOfBullet = 0;
        _towerLedger._numOfAOE = 0;
        _towerLedger._numOfBuff = 0;
        _towerLedger._numOfSlow = 0;
        _towerLedger._numOfLaser = 0;

        Transform child = _missionPanel.GetComponentInChildren<Transform>();

        foreach (Transform iter in child)
        {
            if (iter != _missionPanel.transform)
            {
                Destroy(iter.gameObject);
            }
        }
        _holders = new List<MissionHolder>();
        _holders.Clear();

        InitMissionHolder();
    }

    // tier, towerName, offset: -1(delete), 1(add)
    public void UpdateLedger(Tower.TowerTier tier, string towerName, int offset)
    {
        if (offset == -1)
        {
            switch (tier)
            {
                case Tower.TowerTier.Common:
                    if (_towerLedger._numOfCommon > 0)
                    {
                        _towerLedger._numOfCommon -= 1;
                    }
                    else
                    {
                        _towerLedger._numOfCommon = 0;
                    }
                    break;
                case Tower.TowerTier.Uncommon:
                    if (_towerLedger._numOfUncommon > 0)
                    {
                        _towerLedger._numOfUncommon -= 1;
                    }
                    else
                    {
                        _towerLedger._numOfUncommon = 0;
                    }

                    break;
                case Tower.TowerTier.Rare:
                    if (_towerLedger._numOfRare > 0)
                    {
                        _towerLedger._numOfRare -= 1;
                    }
                    else
                    {
                        _towerLedger._numOfRare = 0;
                    }
                    break;
                case Tower.TowerTier.Unique:
                    if (_towerLedger._numOfUnique > 0)
                    {
                        _towerLedger._numOfUnique -= 1;
                    }
                    else
                    {
                        _towerLedger._numOfUnique = 0;
                    }
                    break;
                case Tower.TowerTier.Legendary:
                    if (_towerLedger._numOfLegendary > 0)
                    {
                        _towerLedger._numOfLegendary -= 1;
                    }
                    else
                    {
                        _towerLedger._numOfLegendary = 0;
                    }
                    break;
                default:
                    break;
            }

            if (towerName.Contains("Bullet"))
            {
                if (_towerLedger._numOfBullet > 0)
                {
                    _towerLedger._numOfBullet -= 1;
                }
                else
                {
                    _towerLedger._numOfBullet = 0;
                }
            }
            else if (towerName.Contains("AOE"))
            {
                if (_towerLedger._numOfAOE > 0)
                {
                    _towerLedger._numOfAOE -= 1;
                }
                else
                {
                    _towerLedger._numOfAOE = 0;
                }
            }
            else if (towerName.Contains("Buff"))
            {
                if (_towerLedger._numOfBuff > 0)
                {
                    _towerLedger._numOfBuff -= 1;
                }
                else
                {
                    _towerLedger._numOfBuff = 0;
                }
            }
            else if (towerName.Contains("Slow"))
            {
                if (_towerLedger._numOfSlow > 0)
                {
                    _towerLedger._numOfSlow -= 1;
                }
                else
                {
                    _towerLedger._numOfSlow = 0;
                }
            }
            else if (towerName.Contains("Laser"))
            {
                if (_towerLedger._numOfLaser > 0)
                {
                    _towerLedger._numOfLaser -= 1;
                }
                else
                {
                    _towerLedger._numOfLaser = 0;
                }
            }
        }
        else
        {
            switch (tier)
            {
                case Tower.TowerTier.Common:
                    _towerLedger._numOfCommon += 1;
                    break;
                case Tower.TowerTier.Uncommon:
                    _towerLedger._numOfUncommon += 1;
                    break;
                case Tower.TowerTier.Rare:
                    _towerLedger._numOfRare += 1;
                    break;
                case Tower.TowerTier.Unique:
                    _towerLedger._numOfUnique += 1;
                    break;
                case Tower.TowerTier.Legendary:
                    _towerLedger._numOfLegendary += 1;
                    break;
                default:
                    break;
            }

            if (towerName.Contains("Bullet"))
            {
                _towerLedger._numOfBullet += 1;
            }
            else if (towerName.Contains("AOE"))
            {
                _towerLedger._numOfAOE += 1;
            }
            else if (towerName.Contains("Buff"))
            {
                _towerLedger._numOfBuff += 1;
            }
            else if (towerName.Contains("Slow"))
            {
                _towerLedger._numOfSlow += 1;
            }
            else if (towerName.Contains("Laser"))
            {
                _towerLedger._numOfLaser += 1;
            }
        }

        _towerTierLedgerText.text = "Common: " + _towerLedger._numOfCommon + "\nUncommon: " + _towerLedger._numOfUncommon + "\nRare: " + _towerLedger._numOfRare
                                + "\nUnique: " + _towerLedger._numOfUnique + "\nLegendary: " + _towerLedger._numOfLegendary;
        _towerTypeLedgerText.text = "Bullet: " + _towerLedger._numOfBullet + "\nAOE: " + _towerLedger._numOfAOE + "\nBuff: " + _towerLedger._numOfBuff
                                + "\nSlow: " + _towerLedger._numOfSlow + "\nLaser: " + _towerLedger._numOfLaser;

        CheckAllMissions();
    }

    #endregion

    #region Mission

    [System.Serializable]
    public struct Mission
    {
        public bool _isCleared;
        public Tower.TowerTier _towerTier;
        public string _towerType;
        public uint _numOfTower;
        public int _reward;   
    }

    // this member displayed on inspector
    [SerializeField] private List<Mission> _missions = null;

    private void CheckAllMissions()
    {
        for (int i = 0; i < _holders.Count; i++)
        {
            if (_holders[i].GetCleared())
            {
                continue;
            }

            if (_holders[i].GetTowerType().Equals(""))
            {
                if (CheckNumOfTowerMission(_holders[i].GetTowerTier(), _holders[i].GetNumOfTower()))
                {
                    Player.GetInstance().AddMoney(_holders[i].GetReward());
                    _holders[i].SetCleared();
                    
                    continue;
                }
            }
            else
            {
                if (CheckTowerTypeMission(_holders[i].GetTowerType(), _holders[i].GetNumOfTower()))
                {
                    Player.GetInstance().AddMoney(_holders[i].GetReward());
                    _holders[i].SetCleared();

                    continue;
                }
            }
        }
    }

    public bool CheckNumOfTowerMission(Tower.TowerTier tier, uint sum)
    {
        switch (tier)
        {
            case Tower.TowerTier.Common:
                if (_towerLedger._numOfCommon == sum)
                {
                    return true;
                }
                break;
            case Tower.TowerTier.Uncommon:
                if (_towerLedger._numOfUncommon == sum)
                {
                    return true;
                }
                break;
            case Tower.TowerTier.Rare:
                if (_towerLedger._numOfRare == sum)
                {
                    return true;
                }
                break;
            case Tower.TowerTier.Unique:
                if (_towerLedger._numOfUnique == sum)
                {
                    return true;
                }
                break;
            case Tower.TowerTier.Legendary:
                if (_towerLedger._numOfLegendary == sum)
                {
                    return true;
                }
                break;
        }

        return false;
    }

    public bool CheckTowerTypeMission(string towerType, uint sum)
    {
        if (towerType.Contains("Bullet"))
        {
            if (_towerLedger._numOfBullet == sum)
            {
                return true;
            }
        }
        else if (towerType.Contains("AOE"))
        {
            if (_towerLedger._numOfAOE == sum)
            {
                return true;
            }
        }
        else if (towerType.Contains("Buff"))
        {
            if (_towerLedger._numOfBuff == sum)
            {
                return true;
            }
        }
        else if (towerType.Contains("Slow"))
        {
            if (_towerLedger._numOfSlow == sum)
            {
                return true;
            }
        }
        else if (towerType.Contains("Laser"))
        {
            if (_towerLedger._numOfLaser == sum)
            {
                return true;
            }
        }

        return false;
    }

    #endregion
}

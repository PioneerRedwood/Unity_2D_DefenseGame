using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    private static MissionManager _instance;
    public static MissionManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new MissionManager();
        }

        return _instance;
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }


    private List<Tower> _towerList;
    private int _missionIndex = -1;

    private bool _firstMissionBool = true;
    private int _firstMissionCount = -1;
    [SerializeField]
    private MissionHolder[] _missions = null;
   
    public int _commonCount = 0;
    public int _uncommonCount = 0;
    public int _rareCount = 0;
    public int _uniqueCount = 0;
    public int _legendaryCount = 0;
    public int _KillCount = 0;


    private void Start()
    {
        InitMission();
        
    }

    private void Update()
    {
        ProceedMission();
    }



    private void InitMission()
    {
        _missionIndex = Random.Range(0, _missions.Length);
        
    }

    private void UpdateTowerList()
    {
        _towerList = Player.GetInstance().GetTowerList();
    }
    
    

    private void ProceedMission()
    {
        if(_missions.Length == 0)
        {
            return;
        }


        if(_firstMissionBool == true)
        {
            Instantiate(_missions[_missionIndex], this.transform);
            _firstMissionBool = false;
        }

    }

    public int GetValue(string s)
    {
    
        return (int)this.GetType().GetField(s).GetValue(this);
    }


}

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MissionHolder : MonoBehaviour
{
    private bool _isCleared;
    private Tower.TowerTier _towerTier;
    private string _towerType;
    private uint _numOfTower;
    private int _reward;

    private string _missionExplanation = null;
    private string _missionName = null;

    public void SetMissionInfo(string missionIdx, MissionManager.Mission mission)
    {
        _missionName = missionIdx;
        StringBuilder stringBuilder = new StringBuilder(missionIdx);
        if (mission._towerType.Equals(""))
        {
            stringBuilder.Append(" Required: " + mission._towerTier + " x " + mission._numOfTower);
        }
        else if (mission._towerType.Equals("Bullet"))
        {
            stringBuilder.Append(" Required: " + mission._towerTier + " " + mission._towerType + " x " + mission._numOfTower);
        }
        else if (mission._towerType.Equals("AOE"))
        {
            stringBuilder.Append(" Required: " + mission._towerTier + " " + mission._towerType + " x " + mission._numOfTower);
        }
        else if (mission._towerType.Equals("Buff"))
        {
            stringBuilder.Append(" Required: " + mission._towerType + " x " + mission._numOfTower);
        }
        else if (mission._towerType.Equals("Slow"))
        {
            stringBuilder.Append(" Required: " + mission._towerType + " x " + mission._numOfTower);
        }
        else if (mission._towerType.Equals("Laser"))
        {
            stringBuilder.Append(" Required: " + mission._towerType + " x " + mission._numOfTower);
        }

        _missionExplanation = stringBuilder.ToString();
        
        _isCleared = mission._isCleared;
        _numOfTower = mission._numOfTower;
        _towerTier = mission._towerTier;
        _towerType = mission._towerType;
        _reward = mission._reward;
    }

    public void OnMissionBtnClicked()
    {
        Player.GetInstance().ShowAlert(_missionExplanation);
    }

    public string GetMissionName()
    {
        return _missionName;
    }

    public Tower.TowerTier GetTowerTier()
    {
        return _towerTier;
    }

    public string GetTowerType()
    {
        return _towerType;
    }

    public uint GetNumOfTower()
    {
        return _numOfTower;
    }
    
    public int GetReward()
    {
        return _reward;
    }    

    public bool GetCleared()
    {
        return _isCleared;
    }
    
    public void SetCleared()
    {
        gameObject.GetComponentInChildren<Text>().text = _missionName + " Cleared";
        //.gameObject.GetComponent<Button>().interactable = false;
        // 텍스트와 색을 바꿔놓는게 좋을듯
        Color color = gameObject.GetComponent<Image>().color;
        color.a = 0.2f;
        Color temp = color;
        gameObject.GetComponent<Image>().color = temp;

        _isCleared = true;
    }


}

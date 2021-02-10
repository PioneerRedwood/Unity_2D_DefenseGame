using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MissionHolder : MonoBehaviour
{
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
    }

    public void OnMissionBtnClicked()
    {
        Player.GetInstance().ShowAlert(_missionExplanation);
    }

    public string GetMissionName()
    {
        return _missionName;
    }
}

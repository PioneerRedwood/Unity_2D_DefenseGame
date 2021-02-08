using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionHolder : MonoBehaviour
{
    enum Mission
    {
        _common,
        _uncommon,
        _rare,
        _unique,
        _legendary
    }

    [SerializeField]
    private int _count = 5;
    [SerializeField]
    private string _text = "";
    [SerializeField]
    private Mission _mission = Mission._common;
    [SerializeField]
    private int _reward = 0;

    private int _missionCount = -1;

    private void Start()
    {
        _text += " " + _count + "개 짓기";
        _missionCount = MissionManager.GetInstance().GetValue(_mission + "Count");
    }

    private void Update()
    {
        
        if (_missionCount != -1 && MissionManager.GetInstance().GetValue(_mission + "Count") - _missionCount >= _count)
        {
            Player.GetInstance().AddMoney(_reward);
            Destroy(this.gameObject);
        }


    }
}

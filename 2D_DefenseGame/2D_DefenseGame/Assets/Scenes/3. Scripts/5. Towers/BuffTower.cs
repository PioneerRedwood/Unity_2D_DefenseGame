using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffTower : Tower
{
    [Header("BuffTower Property")]
    [Range(0, 1)]
    [SerializeField] private float _addDamageBuff = 0;
    [SerializeField] private float _addBuffduration = 0;

    private Tower[] _towers;

    private void Start()
    {
        InvokeRepeating("UpdateTowerList", 0.0f, 0.25f);
    }

    void UpdateTowerList()
    {
        _towers = FindObjectsOfType<Tower>();

        for (int i = _towers.Length - 1; i > -1; i--)
        {
            Vector3 distance = _towers[i].transform.position - transform.position;

            if (distance.magnitude <= base._range)
            {
                if ((transform != _towers[i].transform) && (_towers[i] != null))
                {
                    _towers[i].IncreaseDamage(_addDamageBuff, _addBuffduration);
                }
            }
        }
    }

    public float GetBuffInfo()
    {
        return _addDamageBuff;
    }
}

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

    new void Update()
    {
        base.Update();

        GetTower();

        for (int i = _towers.Length - 1; i > -1; i--)
        {
            Vector3 distance = _towers[i].transform.position - transform.position;

            if (distance.magnitude <= base._range)
            {
                //본인은 적용 X
                if (transform == _towers[i].transform) { }
                else
                {

                    _towers[i].IncreaseDamage(_addDamageBuff, _addBuffduration);
                }
            }
        }
    }

    void Attack()
    {
        for (int i = _enemies.Length; i > -1; --i)
        {
            Vector3 distance = _enemies[i].transform.position - transform.position;

            if (distance.magnitude <= _range)
            {
                _enemies[i].GetComponent<Monster>().OnDamage(_damage);
            }
        }
    }


    void GetTower()
    {
        _towers = GameObject.FindObjectsOfType<Tower>();
    }

}

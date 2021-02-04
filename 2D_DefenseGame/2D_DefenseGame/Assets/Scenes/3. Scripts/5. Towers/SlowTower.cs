using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTower : Tower
{
    [Header("SlowTower property")]
    [Range(0, 1)]
    [SerializeField] private float _slow = 0.0f;

    new void Update()
    {
        base.Update();

        for (int i = 0; i < _enemies.Length; i++)
        {
            if (Vector3.Distance(transform.position, _enemies[i].transform.position) < _range)
            {
                _enemies[i].GetComponent<Monster>().DecreaseSpeed(transform, _slow, _range);
            }
            else
            {
                if (_enemies[i].GetComponent<Monster>().GetDebuffedTowerTransform() == transform)
                {
                    _enemies[i].GetComponent<Monster>().ResetBuff();
                }
            }
        }

    }
}

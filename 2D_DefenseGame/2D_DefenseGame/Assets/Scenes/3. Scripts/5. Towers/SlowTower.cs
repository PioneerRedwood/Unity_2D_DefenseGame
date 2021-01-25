using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTower : Tower
{

    [SerializeField]
    private float _slow = 0.0f;
    private Monster[] _enemies = null;

    new void Update()
    {
        base.Update();



    }



    private void GetEnemy()
    {
        for(int i = base._enemies.Length ; i > -1; --i)
        {
            _enemies[i] = base._enemies[i].GetComponent<Monster>();
        }
    }

    // 함수 하나로 float Vector2.Distance(Vector2 x, Vector2 y)
    private float GetDistance(Vector2 x, Vector2 y)
    {
        return Vector2.Distance(x, y);
        //float xm = x.x - y.x;
        //float ym = x.y - y.y;

        //return Mathf.Sqrt(Mathf.Pow(xm, 2) + Mathf.Pow(ym, 2));
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Monster _target;
    public float _speed = 1.0f;
    public float _damage = 20.0f;

    private float _deltaTime = 0.0f;
    private float _destroyDelay = 1.0f;
    
    void Update()
    {
        if (_target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);

            // 충돌 검사하는 부분
            // 몬스터 크기에 맞게 충돌해야 함
            if (transform.position == _target.transform.position)
            {
                _target.OnDamage(_damage);
                Destroy(gameObject);
            }
        }
        else
        {
            _deltaTime += Time.deltaTime;
            if(_destroyDelay <= _deltaTime)
            {
                Destroy(gameObject);
            }
            return;
        }
    }

    public bool SetTarget(Monster target)
    {
        if(target != null)
        {
            _target = target;
            return true;
        }
        else
        {
            return false;
        }
    }
}

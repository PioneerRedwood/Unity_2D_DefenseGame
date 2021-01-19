using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet")]
    private float _speed = 1.0f;
    private float _damage = 20.0f;

    private Monster _target;
    private float _deltaTime = 0.0f;

    void Update()
    {
        if (_target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);

            // 충돌 검사하는 부분
            // 몬스터 크기에 맞게 충돌해야 함
            if (GetDistance(new Vector2(transform.position.x, transform.position.y), 
                new Vector2(_target.transform.position.x, _target.transform.position.y)) < 1.0f )
            {
                _target.OnDamage(_damage);
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool SetTarget(Monster target)
    {
        if (target != null)
        {
            _target = target;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SetDamage(float input)
    {
        _damage = input;
    }

    public void SetSpeed(float input)
    {
        _speed = input;
    }

    private float GetDistance(Vector2 x, Vector2 y)
    {
        float xm = x.x - y.x;
        float ym = x.y - y.y;

        return Mathf.Sqrt(Mathf.Pow(xm, 2) + Mathf.Pow(ym, 2));
    }

}

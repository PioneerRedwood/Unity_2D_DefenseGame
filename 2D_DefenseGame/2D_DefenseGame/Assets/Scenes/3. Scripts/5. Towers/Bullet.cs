using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet")]
    private float _speed = 1.0f;
    private float _damage = 20.0f;

    private Monster _target;

    void Update()
    {
        if (_target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);

            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), 
                new Vector2(_target.transform.position.x, _target.transform.position.y)) <= 0.25f )
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
}

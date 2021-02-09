using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEBullet : MonoBehaviour
{
    [Header("AOEBullet")]
    [SerializeField] private float _speed = 0.5f;
    [SerializeField] private float _damage = 20.0f;
    [SerializeField] private float _aoeRange = 3;
   
    private Monster _target;
    private GameObject[] _enemies;
    private string _enemyTag = "Enemy";

    private void Update()
    {
        if (_target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);

            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y),
                new Vector2(_target.transform.position.x, _target.transform.position.y)) < 0.25f)
            {

                _enemies = GameObject.FindGameObjectsWithTag(_enemyTag);
                for (int i = _enemies.Length - 1; i > -1; i--)
                {
                    if (Vector2.Distance(new Vector2(_target.transform.position.x, _target.transform.position.y), new Vector2(_enemies[i].transform.position.x, _enemies[i].transform.position.y)) < _aoeRange)
                    {
                        _enemies[i].transform.GetComponent<Monster>().OnDamage(_damage);
                    }
                }
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

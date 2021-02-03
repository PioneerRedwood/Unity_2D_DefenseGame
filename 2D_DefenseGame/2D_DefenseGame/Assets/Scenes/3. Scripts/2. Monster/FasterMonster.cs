using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// summary:
//    기본 이동속도가 빠르지만 공격 받으면 둔화
//
public class FasterMonster : Monster
{
    [Header("FasterMonster property")]
    [Range(0, 1)]
    [SerializeField] private float _delay = 1.0f;
    [SerializeField] private float _delayTime = 0.2f;

    private float _currDelay = 0.0f;
    private bool _isAttacked = false;
    private float _deltaTime = 0f;
    private float _damagedTime = 0f;

    // FasterMonster 전용 이동 함수
    new void MoveToNext()
    {
        if (_isDirect)
        {
            Quaternion direction = Quaternion.LookRotation(Vector3.forward, new Vector3(_nextPos.x, _nextPos.y, transform.position.z) - transform.position);
            _objectSprite.transform.rotation = direction;

            _currSpeed = _speed - _speed * _currSlow - _speed * _currDelay;

            transform.position = Vector2.MoveTowards(transform.position, (Vector2)_nextPos, _currSpeed * Time.deltaTime);

            if ((Vector2)transform.position == _nextPos && _nextPos != _waypoints[_waypoints.Length - 1])
            {
                _nextPos = _waypoints[++_wayPointIdx];
            }

            if ((Vector2)transform.position == _waypoints[_waypoints.Length - 1])
            {
                Destroy(gameObject);
                Player.GetInstance().LoseLife(1);
            }
        }
        else
        {
            _currSpeed = _speed - _speed * _currSlow - _speed * _currDelay;
            if (_currSpeed < _speed * 0.3)
            {
                _currSpeed = _speed * 0.3f;
            }

            transform.position = Vector2.MoveTowards(transform.position, (Vector2)_nextPos, _currSpeed * Time.deltaTime);

            if ((Vector2)transform.position == _nextPos && _nextPos != _waypoints[_waypoints.Length - 1])
            {
                _nextPos = _waypoints[++_wayPointIdx];
            }

            if ((Vector2)transform.position == _waypoints[_waypoints.Length - 1])
            {
                Destroy(gameObject);
                Player.GetInstance().LoseLife(1);
            }
        }
    }


    public override void OnDamage(float damage)
    {
        _currHP -= damage;
        _isAttacked = true;
        _damagedTime = _deltaTime;
        _currDelay = _delay;

    }

    protected override void ShowHP()
    {
        _currentHPPref.fillAmount = _currHP / _hp;
    }

    void Start()
    {
        _currHP = _hp;
        _currSpeed = _speed;
    }

    void Update()
    {
        _deltaTime += Time.deltaTime;

        if(_isAttacked && ((_deltaTime - _damagedTime) >= _delayTime))
        {
            _isAttacked = false;
            _currDelay = 0.0f;
            _deltaTime = 0;
            _damagedTime = 0;
        }

        MoveToNext();

        ShowHP();
        if (_currHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}

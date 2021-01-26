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

    private bool _isAttacked = false;
    private float _deltaTime = 0f;
    private float _damagedTime = 0f;

    public override void OnDamage(float damage)
    {
        _currHP -= damage;
        _isAttacked = true;
        _damagedTime = _deltaTime;
        if (_currSpeed > _speed * _delay)
        {
            _currSpeed = _currSpeed * _delay;
        }
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
            _currSpeed = _speed;
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

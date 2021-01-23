using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// summary:
//    기본 이동속도가 빠르지만 공격 받으면 둔화
//
public class FasterMonster : Monster
{
    [Header("FasterMonster property")]
    [SerializeField] private float _hp = 300f;
    [SerializeField] private float _speed = 0;

    private float _currHP = 0f;

    private float _basicSpeed = 0f;
    private bool _isAttacked = false;
    private float _deltaTime = 0f;
    private float _damagedTime = 0f;

    // 둔화 시간과 정도
    [SerializeField] private float _delayTime = 0.2f;
    [SerializeField] private float _delay = 0.95f;

    public override void OnDamage(float damage)
    {
        _currHP -= damage;
        _isAttacked = true;
        _damagedTime = _deltaTime;
        _speed *= _delay;
    }

    protected override void ShowHP()
    {
        _currentHPPref.fillAmount = _currHP / _hp;
    }

    void Start()
    {
        _currHP = _hp;
        _basicSpeed = _speed;
    }

    void Update()
    {
        _deltaTime += Time.deltaTime;
        if(_isAttacked && ((_deltaTime - _damagedTime) >= _delayTime))
        {
            _isAttacked = false;
            _speed = _basicSpeed;
            _deltaTime = 0;
            _damagedTime = 0;
        }

        MoveToNext(_speed);
        ShowHP();
        if (_currHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}

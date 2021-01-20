using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// summary:
//    평상시엔 이동속도가 빠르지만 공격 받으면 이동속도가 둔화
public class FasterMonster : Monster
{
    [Header("FasterMonster property")]
    [SerializeField] private float _hp = 300f;
    [SerializeField] private float _speed = 0;

    private float _basicSpeed = 0f;
    private bool _isAttacked = false;
    private float _deltaTime = 0f;
    private float _delayTime = 1f;

    public override void OnDamage(float damage)
    {
        _hp -= damage;
        _isAttacked = true;
        _deltaTime = Time.timeScale;
        _speed = _speed * 0.8f;
    }

    void Start()
    {
        _basicSpeed = _speed;
    }

    void Update()
    {
        if(_isAttacked && Time.timeScale - _deltaTime >= _delayTime)
        {
            _isAttacked = false;
            _speed = _basicSpeed;
        }

        MoveToNext(_speed);
        //ShowHP(_hp);
        if (_hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}

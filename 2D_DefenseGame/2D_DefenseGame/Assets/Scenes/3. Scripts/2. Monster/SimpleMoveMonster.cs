using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMoveMonster : Monster
{
    [Header("SimpleMoveMonster property")]
    [SerializeField] private float _hp = 300f;
    [SerializeField] private float _speed = 0;

    private float _currHP = 0f;

    public override void OnDamage(float damage)
    {
        _currHP -= damage;
    }

    protected override void ShowHP()
    {
        _hpPref.fillAmount = _currHP / _hp;
    }

    private void Start()
    {
        _currHP = _hp;
    }

    void Update()
    {
        MoveToNext(_speed);
        ShowHP();

        if (_currHP <= 0)
        {
            Destroy(gameObject);
            DestroyMonster();
        }
    }
}

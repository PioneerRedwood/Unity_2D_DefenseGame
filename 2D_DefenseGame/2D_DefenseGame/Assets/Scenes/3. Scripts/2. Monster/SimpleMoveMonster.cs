using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMoveMonster : Monster
{
    public override void OnDamage(float damage)
    {
        _currHP -= damage;
    }

    protected override void ShowHP()
    {
        _currentHPPref.fillAmount = _currHP / _hp;
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
        }
    }
}

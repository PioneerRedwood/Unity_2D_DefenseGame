﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMoveMonster : Monster
{
    [Header("SimpleMoveMonster property")]
    [SerializeField] private float _hp = 300f;
    [SerializeField] private float _speed = 0;

    public override void OnDamage(float damage)
    {
        _hp -= damage;
    }
    
    void Update()
    {
        MoveToNext(_speed);
        ShowHP(_hp);

        if (_hp <= 0)
        {
            Destroy(gameObject);
            DestroyMonster();
        }
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Tile
{
    public enum TowerTier
    {
        Common,
        Uncommon,
        Rare,
        Unique,
        Legendary
    }

    [Header("Basic Tower Property")]
    public TowerTier _tier;
    public string _towerName;
    public float _defaultDamage = 0.0f;
    public float _range = 2f;

    public float _damage = 1.0f;

    protected Transform _targetTransform;
    protected Monster _currTarget;
    protected string _enemyTag = "Enemy";
    protected float _turnSpeed = 10f;
    protected GameObject[] _enemies = null;

    private float _stateChangedTime = 0.0f;
    private float _increaseDamage = 0;
    private float _decreaseDamage = 0;
    private float _duration = 0.0f;
    private float _stateChangedbuffTime = 0.0f;
    private float _buffduration = 0.0f;

    void Awake()
    {
        _damage = _defaultDamage;
    }

    protected void Update()
    {
        UpdateTarget();
    }

    #region Updating target
    void UpdateTarget()
    {
        // 태그로 적들을 배열로 가져옴
        _enemies = GameObject.FindGameObjectsWithTag(_enemyTag);
        GameObject nearestEnemy = null;

        float shortestDistance = Mathf.Infinity;

        // 필드 내 적 중에서 가장 가까운 놈을 찾아내서 등록
        foreach (GameObject enemy in _enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        // 적을 찾은 경우 타겟 수정
        if (nearestEnemy != null && shortestDistance <= _range)
        {
            _targetTransform = nearestEnemy.transform;
            _currTarget = nearestEnemy.GetComponent<Monster>();
        }
        else
        {
            _targetTransform = null;
            _currTarget = null;
        }


        if (_stateChangedbuffTime != 0.0f && ((Time.time - _stateChangedbuffTime) >= _buffduration))
        {
            ResetDamage(true);
        }

        if (_stateChangedTime != 0.0f && ((Time.time - _stateChangedTime) >= _duration))
        {
            ResetDamage(false);
        }

        LoadDamage();
    }
    #endregion

    #region Control Damage

    public List<float> GetState()
    {
        List<float> states = new List<float>();
        if(_increaseDamage != 0.0f)
        {
            states.Add(_increaseDamage);
        }
        else
        {
            states.Add(0.0f);
        }

        if(_decreaseDamage != 0.0f)
        {
            states.Add(_decreaseDamage);
        }
        else
        {
            states.Add(0.0f);
        }

        return states;
    }

    public void DecreaseDamage(float value, float duration)
    {
        if (value >= _decreaseDamage)
        {
            _decreaseDamage = value;
            _duration = duration;
            _stateChangedTime = Time.time;

            LoadDamage();
        }
    }

    public void IncreaseDamage(float value, float duration)
    {
        if (value >= _increaseDamage )
        {
            _increaseDamage = value;
            _buffduration = duration;
            _stateChangedbuffTime  = Time.time;

            LoadDamage();
        }
    }

    public void LoadDamage()
    {
        _damage = _defaultDamage + _defaultDamage * _increaseDamage - _defaultDamage * _decreaseDamage;
    }

    public void LoadAllBufferTower()
    {

    }

    public void ResetDamage(bool selector)
    {
        _damage = _defaultDamage;

        if (selector)
        {
            _increaseDamage = 0.0f;
            _buffduration = 0.0f;
            _stateChangedbuffTime = 0.0f;

        }
        else if (!selector)
        {
            _decreaseDamage = 0.0f;
            _duration = 0.0f;
            _stateChangedTime = 0.0f;
        }
    }

    #endregion

    #region Draw Range of tower
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);
    }

    #endregion
}

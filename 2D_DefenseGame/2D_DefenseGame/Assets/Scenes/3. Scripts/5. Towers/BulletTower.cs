﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTower : Tower
{
    [Header("BulletTower Property")]
    [SerializeField] private Bullet _bullet = null;
    [SerializeField] private Transform _muzzle = null;

    [Header("Attacking")]
    [SerializeField] private float _bulletSpeed = 20.0f;
    [SerializeField] private float _attackDelay = 1f;
    [SerializeField] private bool _isRotate = true;

    protected float _fireCount = 0f;

    private void Update()
    {
        if (_targetTransform == null)
        {
            if (_isRotate)
            {
                transform.Rotate(new Vector3(0f, 0f, 1f) * 80 * Time.deltaTime, Space.Self);
            }
            return;
        }
        else
        {
            if (_isRotate)
            {
                Quaternion direction = Quaternion.LookRotation(Vector3.forward, _targetTransform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, direction, _turnSpeed * Time.deltaTime);
            }

            if (_attackDelay <= _fireCount)
            {
                Attack();
                _fireCount = 0f;
            }
            _fireCount += Time.deltaTime;
        }
    }

    #region Attacking process
    void Attack()
    {
        var tempBullet = Instantiate(_bullet, _muzzle.transform);

        tempBullet.SetDamage(_damage);
        tempBullet.SetSpeed(_bulletSpeed);
        tempBullet.SetTarget(_currTarget);
    }
    #endregion

}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : Tower
{
    [Header("LasertTower Property")]
    public Transform _muzzle;
    public LineRenderer _lineRenderer;
    public Transform _hitEffect;
    public LayerMask _targetLayer;

    [Header("Attacking")]
    public float _bulletSpeed = 20.0f;
    public float _attackDelay = 1f;

    protected float _fireCount = 0f;


    void Update()
    {
        if (_targetTransform == null)
        {
            transform.Rotate(new Vector3(0f, 0f, 0.5f), Space.Self);
            DisableLaser();

            return;
        }
        else
        {
            Quaternion direction = Quaternion.LookRotation(Vector3.forward, _targetTransform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, direction, _turnSpeed * Time.deltaTime);

            EnableLaser();
            SpawnLaser();
         }
    }


    private void SpawnLaser()
    {
        Vector3 direction = _currTarget.transform.position - _muzzle.position;
        RaycastHit2D[] hit = Physics2D.RaycastAll(_muzzle.position, direction, _range, _targetLayer);


        for (int i = 0; i <hit.Length; i++)
        {
            if(hit[i].transform == _currTarget.transform)
            {

                _lineRenderer.SetPosition(0, _muzzle.position);
                _lineRenderer.SetPosition(1, new Vector3(hit[i].point.x, hit[i].point.y, 0) + Vector3.back);

                _hitEffect.position = hit[i].point;
                _currTarget.OnDamage(_damage * Time.deltaTime);
             }
         }
     }

    private void EnableLaser()
    {
        _lineRenderer.gameObject.SetActive(true);
        _hitEffect.gameObject.SetActive(true);
    }

    private void DisableLaser()
    {
        _lineRenderer.gameObject.SetActive(false);
        _hitEffect.gameObject.SetActive(false);
    }






}
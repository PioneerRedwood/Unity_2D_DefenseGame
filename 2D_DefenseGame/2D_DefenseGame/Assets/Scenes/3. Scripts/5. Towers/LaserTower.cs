using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : Tower
{
    [Header("LasertTower Property")]
    [SerializeField] private Transform _muzzle = null;
    [SerializeField] private LineRenderer _lineRenderer = null;
    [SerializeField] private Transform _hitEffect = null;
    [SerializeField] private LayerMask _targetLayer = 0;
    [SerializeField] private bool _isRotate = true;

    void Update()
    {
        if (_targetTransform == null)
        {
            if (_isRotate)
            {
                transform.Rotate(new Vector3(0f, 0f, 1f) * 80 * Time.deltaTime, Space.Self);
            }
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

    #region Laser attack

    private void SpawnLaser()
    {
        Vector3 direction = _currTarget.transform.position - _muzzle.position;
        RaycastHit[] hit = Physics.RaycastAll(new Vector3(transform.position.x, transform.position.y, -1f), new Vector3(direction.x, direction.y, 0f),
                                                _range, _targetLayer);

        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, -1f), new Vector3(direction.x, direction.y, 0f));

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].transform == _currTarget.transform)
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
    #endregion


}

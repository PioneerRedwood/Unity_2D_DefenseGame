using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTower : Tower
{
    void Update()
    {

        if (_targetTransform == null)
        {
            transform.Rotate(new Vector3(0f, 0f, 0.5f), Space.Self);
            return;
        }
        else
        {
            // 지정된 적 방향으로 회전
            Quaternion direction = Quaternion.LookRotation(Vector3.forward, _targetTransform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, direction, _turnSpeed * Time.deltaTime);

            //DrawLine();
            // Attack
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

        Debug.Log("shoot");
        Debug.Log(tempBullet);

        tempBullet.SetDamage(_damage);
        tempBullet.SetSpeed(_bulletSpeed);
        tempBullet.SetTarget(_currTarget);
    }

    #endregion

}

using System.Collections;
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
    public TowerTier _tier;
    public string _towerName;

    [Header("Tower Property")]
    public Bullet _bullet;
    public Transform _muzzle;

    [Header("Attacking")]
    public float _range = 2f;
    public float _damage = 1.0f;
    public float _bulletSpeed = 20.0f;
    public float _attackDelay = 1f;

    protected Transform _targetTransform;
    protected Monster _currTarget;

    protected string _enemyTag = "Enemy";
    protected float _turnSpeed = 10f;
    protected float _fireCount = 0f;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        Debug.Log("Tower Class");
    }

    #region Updating target
    void UpdateTarget()
    {
        // 태그로 적들을 배열로 가져옴
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(_enemyTag);
        GameObject nearestEnemy = null;

        float shortestDistance = Mathf.Infinity;

        // 필드 내 적 중에서 가장 가까운 놈을 찾아내서 등록
        foreach (GameObject enemy in enemies)
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
    }
    #endregion


    //void Update()
    //{
        // 타겟이 없으면 돌기만 함
        //if (_targetTransform == null)
        //{
        //    transform.Rotate(new Vector3(0f, 0f, 0.5f), Space.Self);
        //    return;
        //}
        //else
        //{
        //    // 지정된 적 방향으로 회전
        //    Quaternion direction = Quaternion.LookRotation(Vector3.forward, _targetTransform.position - transform.position);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, direction, _turnSpeed * Time.deltaTime);

        //    //DrawLine();
        //    // Attack
        //    if (_attackDelay <= _fireCount)
        //    {
        //        Attack();
        //        _fireCount = 0f;
        //    }
        //    _fireCount += Time.deltaTime;
        //}
    //}



    #region Draw Range of tower
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
    
    #endregion
}

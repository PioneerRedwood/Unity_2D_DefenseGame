using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Tile
{
    private Transform _targetTransform;
    private Monster _currTarget;
    public float _range = 2f;

    public string _enemyTag = "Enemy";
    public float _turnSpeed = 10f;

    public Bullet _bullet;
    public Transform _muzzle;
    public float _attackDelay = 1f;
    private float _fireCount = 0f;

    public enum towerTier
    {
        common,
        uncommon,
        rare,
        unique,
        legendary
    }
    public towerTier Tier;
    public string towerName;

    private LineRenderer line;
    void Start()
    {
        line = GetComponent<LineRenderer>();

        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    #region Updating target
    void UpdateTarget()
    {
        // 태그로 적들을 배열로 가져옴
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(_enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        // 필드 내 적 중에서 가장 가까운 놈을 찾아내서 등록
        foreach(GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
            if(distanceToEnemy < shortestDistance)
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
    

    void Update()
    {
        // 타겟이 없으면 돌기만 함
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

    // 공격 기능 타워를 만들어야함
    #region Attacking process
    protected void Attack()
    {
        var tempBullet = Instantiate(_bullet, _muzzle.transform);
        tempBullet.SetTarget(_currTarget);
    }

    #endregion

    #region Draw Range of tower
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
    
    // 원활히 작동 안됨
    private void DrawLine()
    {
        if(_targetTransform != null)
        {
            line.startColor = Color.red;
            line.SetPosition(0, _muzzle.transform.position);
            line.SetPosition(1, _targetTransform.position);
            line.startWidth = 0.1f;
            line.endWidth = 0.1f;
        }
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Tile
{
    private Transform target;
    public float range = 2f;

    public string enemyTag = "Enemy";
    public float turnSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(new Vector3(0, 0, 50));
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        // 태그로 적 배열을 가져옴
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
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
        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 타겟이 없으면 아무것도 안함
        if (target == null)
        {
            transform.Rotate(new Vector3(0f, 0f, 0.3f), Space.Self);
            return;
        }
        else
        {
            // 
            Quaternion _rotation = Quaternion.LookRotation(Vector3.forward, target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, _rotation, turnSpeed * Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

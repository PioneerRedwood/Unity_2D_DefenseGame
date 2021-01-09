using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    protected enum MonsterState
    {
        moving, stop, dead, normal
    }
    [SerializeField] private float hp = 0;
    [SerializeField] private float speed = 0;

    protected MonsterState state = MonsterState.normal;
    protected Vector2Int[] Waypoint { get; set; }
    protected int wayPointIdx = 0;
    protected Vector2Int nextPos;
    
    #region Damage
    public abstract bool OnDamage(float damage);

    public void DestroyMonster()
    {
        if (state != MonsterState.dead)
        {
            state = MonsterState.dead;
            Destroy(this);
        }
    }
    #endregion

    #region Waypoint : Basic movement
    public void InitWaypoint(Vector2Int[] waypoints)
    {
        Waypoint = waypoints;
        nextPos = Waypoint[wayPointIdx + 1];
    }

    // 이동 유튜브 참고
    // https://www.youtube.com/watch?v=ExRQAEm4jPg&ab_channel=AlexanderZotov
    protected void MoveToNext()
    {
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)nextPos, speed * Time.deltaTime);

        if ((Vector2)transform.position == nextPos && nextPos != Waypoint[Waypoint.Length - 1])
        {
            nextPos = Waypoint[++wayPointIdx];
        }

        if ((Vector2)transform.position == Waypoint[Waypoint.Length - 1])
        {
            Destroy(gameObject);
        }
    }
    #endregion

}

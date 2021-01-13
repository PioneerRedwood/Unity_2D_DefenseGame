using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{ 
    // 몬스터 상태 enum
    // 용도?
    protected enum MonsterState
    {
        moving, stop, dead, normal
    }
    protected MonsterState _state = MonsterState.normal;

    [SerializeField] private float speed = 0;
    protected Vector2Int[] _waypoints { get; set; }
    protected int _wayPointIdx = 0;
    protected Vector2Int _nextPos;
    
    #region Damage
    public abstract void OnDamage(float damage);
    #endregion

    #region Waypoint movement
    public void InitWaypoint(Vector2Int[] waypoints)
    {
        _waypoints = waypoints;
        _nextPos = _waypoints[_wayPointIdx + 1];
    }

    // 이동 유튜브 참고
    // https://www.youtube.com/watch?v=ExRQAEm4jPg&ab_channel=AlexanderZotov
    protected void MoveToNext()
    {
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)_nextPos, speed * Time.deltaTime);

        if ((Vector2)transform.position == _nextPos && _nextPos != _waypoints[_waypoints.Length - 1])
        {
            _nextPos = _waypoints[++_wayPointIdx];
        }

        if ((Vector2)transform.position == _waypoints[_waypoints.Length - 1])
        {
            Destroy(gameObject);
        }
    }
    #endregion
}

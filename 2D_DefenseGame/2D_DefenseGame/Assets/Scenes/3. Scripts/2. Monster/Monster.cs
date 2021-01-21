using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Monster : MonoBehaviour
{
    [Header("Monster Property")]
    public Image _hpPref = null;

    protected Vector2Int[] _waypoints { get; set; }
    protected Vector2Int _nextPos;
    protected int _wayPointIdx = 0;
    
    #region Damage
    public abstract void OnDamage(float damage);

    protected abstract void ShowHP();

    protected void DestroyMonster()
    {
        Destroy(gameObject);
    }
    #endregion

    #region Waypoint movement
    public void InitWaypoint(Vector2Int[] waypoints)
    {
        _waypoints = waypoints;
        _nextPos = _waypoints[_wayPointIdx + 1];
    }

    // 이동 유튜브 참고
    // https://www.youtube.com/watch?v=ExRQAEm4jPg&ab_channel=AlexanderZotov
    protected void MoveToNext(float speed)
    {
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)_nextPos, speed * Time.deltaTime);

        if ((Vector2)transform.position == _nextPos && _nextPos != _waypoints[_waypoints.Length - 1])
        {
            _nextPos = _waypoints[++_wayPointIdx];
        }

        if ((Vector2)transform.position == _waypoints[_waypoints.Length - 1])
        {
            Destroy(gameObject);
            Player.GetInstance().LoseLife(1);
        }
    }
    #endregion
}

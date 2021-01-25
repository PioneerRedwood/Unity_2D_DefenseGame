using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Monster : MonoBehaviour
{
    [Header("Monster Property")]
    public Image _currentHPPref = null;
    [SerializeField] protected float _hp = 0.0f;
    [SerializeField] protected float _speed = 0.0f;

    protected float _currHP = 0.0f;
    protected Vector2[] _waypoints { get; set; }
    protected Vector2 _nextPos;
    protected int _wayPointIdx = 0;
    
    #region Damage

    public abstract void OnDamage(float damage);

    protected abstract void ShowHP();

    #endregion

    #region Waypoint movement

    public void InitWaypoint(Vector2[] waypoints)
    {
        _waypoints = waypoints;
        _nextPos = _waypoints[_wayPointIdx + 1];
    }
    
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

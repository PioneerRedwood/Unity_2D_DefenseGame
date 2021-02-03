using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Monster : MonoBehaviour
{
    [Header("Monster Property")]
    public Image _currentHPPref = null;
    public string _monsterName = "";
    [SerializeField] protected GameObject _objectSprite = null;
    [SerializeField] protected float _hp = 0.0f;
    [SerializeField] protected float _speed = 0.0f;
    [SerializeField] protected bool _isDirect = false;

    protected float _currSpeed = 0.0f;
    protected float _currHP = 0.0f;

    protected float _currSlow = 0.0f;
    protected float _range = 0.0f;
    
    public Transform _giveBuff = null;
    protected Vector2[] _waypoints { get; set; }
    protected Vector2 _nextPos;
    protected int _wayPointIdx = 0;

    private void Awake()
    {
        _currSpeed = _speed;
        _currHP = _hp;
    }

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

    protected void MoveToNext()
    {
        if (_isDirect)
        {
            Quaternion direction = Quaternion.LookRotation(Vector3.forward ,new Vector3(_nextPos.x, _nextPos.y, transform.position.z) - transform.position);
            _objectSprite.transform.rotation = direction;

            _currSpeed = _speed - _speed * _currSlow;

            transform.position = Vector2.MoveTowards(transform.position, (Vector2)_nextPos, _currSpeed * Time.deltaTime);

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
        else
        {
            _currSpeed = _speed - _speed * _currSlow;

            transform.position = Vector2.MoveTowards(transform.position, (Vector2)_nextPos, _currSpeed * Time.deltaTime);

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
    }
    #endregion

    #region Control Properties

    public float GetCurrHP()
    {
        return _currHP;
    }

    public float GetcurrSpeed()
    {
        return _currSpeed;
    }

    public float GetSpeed()
    {
        return _speed;
    }

    public void SetSpeed(float input)
    {
        _currSpeed = input;
    }

    public float GetState()
    {
        return _currSlow;
    }

    public void DecreaseSpeed(Transform buffer, float slow, float range)
    {
        if (_giveBuff != buffer)
        {
            if (_currSlow < slow)
            {
                _range = range;
                _currSlow = slow;
                _giveBuff = buffer;
            }
        }
    }

    public void ResetBuff()
    {
        _giveBuff = null;
        _currSlow = 0.0f;
    }

    #endregion
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Monster : MonoBehaviour
{
    [Header("Monster Property")]
    [SerializeField] protected string _monsterName = null;
    [SerializeField] protected Image _currHPImg = null;
    [SerializeField] protected GameObject _objectSprite = null;
    [SerializeField] protected float _hp = 0.0f;
    [SerializeField] protected float _speed = 0.0f;
    [SerializeField] protected bool _isDirect = false;

    protected float _currSpeed = 0.0f;
    protected float _currHP = 0.0f;

    protected float _currSlow = 0.0f;
    protected float _range = 0.0f;
    
    protected Transform _debuffedTowerTransform = null;
    protected Vector2[] _waypoints { get; set; }
    protected Vector2 _nextPos;
    protected int _wayPointIdx = 0;

    private Obstacle _obstacle;

    private void Awake()
    {
        _currSpeed = _speed;
        _currHP = _hp;
        InvokeRepeating("UpdateObstacle", 0.0f, 0.25f);
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
            Quaternion direction = Quaternion.LookRotation(Vector3.forward, new Vector3(_nextPos.x, _nextPos.y, transform.position.z) - transform.position);
            _objectSprite.transform.rotation = direction;

            if (_obstacle == null)
            {
                _currSpeed = _speed - _speed * _currSlow;
            }

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
            if (_obstacle == null)
            {
                _currSpeed = _speed - _speed * _currSlow;
            }

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

    public string GetName()
    {
        return _monsterName;
    }

    public float GetCurrHP()
    {
        return _currHP;
    }

    public float GetCurrSpeed()
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

    public Vector2 GetNextPos()
    {
        return _nextPos;
    }

    public float GetState()
    {
        return _currSlow;
    }

    public void DecreaseSpeed(Transform buffer, float slow, float range)
    {
        if (_debuffedTowerTransform != buffer)
        {
            if (_currSlow < slow)
            {
                _range = range;
                _currSlow = slow;
                _debuffedTowerTransform = buffer;
            }
        }
    }

    public Transform GetDebuffedTowerTransform()
    {
        return _debuffedTowerTransform;
    }

    public void ResetBuff()
    {
        _debuffedTowerTransform = null;
        _currSlow = 0.0f;
    }

    #endregion

    #region Handling Obstacle

    private void UpdateObstacle()
    {
        if(_obstacle != null)
        {
            _currSpeed = 0.0f;
            StartCoroutine("EngageObstacle");
        }
        else
        {
            _currSpeed = _speed;
            StopCoroutine("EngageObstacle");
        }
    }

    public void SetObstacle(Obstacle obstacle)
    {
        if (obstacle != null)
        {
            _obstacle = obstacle;
            return;
        }
    }

    public bool GetObstacle()
    {
        if(_obstacle != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator EngageObstacle()
    {
        _obstacle.GetDamage(1);
        yield return new WaitForSeconds(0.25f);
    }

    #endregion
}

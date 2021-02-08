using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// obstacle attached upon a Route

public class Obstacle : Tile
{
    [Header("Basic Obstacle Property")]
    [SerializeField] private Image _currHPImg = null;
    [SerializeField] private string _name = null;
    [SerializeField] private float _HP = 0.0f;
    [SerializeField] private float _distance = 0.0f;

    private float _currHP = 0.0f;

    private Route _route = null;

    private List<Monster> _monsters;
    private RaycastHit _hit;

    void Awake()
    {
        _currHP = _HP;
        _monsters = new List<Monster>();
        InvokeRepeating("UpdateEnemyList", 0.1f, 0.1f);
    }

    void Update()
    {
        foreach (Monster monster in _monsters)
        {
            if(monster == null)
            {
                continue;
            }

            // Ray와 Line의 차이
            bool ray = Physics.Raycast(new Vector3(monster.transform.position.x, monster.transform.position.y, -0.2f),
                                       new Vector3(monster.GetNextPos().x - monster.transform.position.x, monster.GetNextPos().y - monster.transform.position.y, -0.2f), 
                                       out _hit, 15.0f);
            
            Debug.DrawRay(new Vector3(monster.transform.position.x, monster.transform.position.y, -0.2f),
                           new Vector3(monster.GetNextPos().x, monster.GetNextPos().y, -0.2f));
            
            if (ray && _hit.transform.CompareTag("Obstacle"))
            {
                if (!monster.GetObstacle())
                {
                    monster.SetObstacle(this);
                }
            }
        }

        if (_currHP <= 0.0f)
        {
            if (_route != null)
            {
                _route.SetObstacleBuilt(false);
            }
            Destroy(gameObject);
        }
    }

    private void UpdateEnemyList()
    {
        Monster[] monsters = FindObjectsOfType<Monster>();

        foreach (Monster monster in monsters)
        {
            if ((Vector2.Distance(monster.transform.position, transform.position) <= _distance))
            {
                if (!_monsters.Contains(monster))
                {
                    _monsters.Add(monster);
                }
            }
            else
            {
                if (_monsters.Contains(monster) || (monster == null))
                {
                    monster.SetObstacle(null);
                    _monsters.Remove(monster);
                }
            }
        }
    }

    public void InitObstacleOnRoute(Route route)
    {
        if (_route == null)
        {
            _route = route;
        }
    }

    public float GetCurrHP()
    {
        return _currHP;
    }

    public void GetDamage(float damage)
    {
        _currHP -= damage;
        _currHPImg.fillAmount = _currHP / _HP;
    }

    public void SetName(string name)
    {
        _name = name;
    }

    public string GetName()
    {
        return _name;
    }

    public int GetMonsterCount()
    {
        return _monsters.Count;
    }
}

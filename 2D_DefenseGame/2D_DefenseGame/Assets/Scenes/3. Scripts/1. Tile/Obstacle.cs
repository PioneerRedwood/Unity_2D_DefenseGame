using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// obstacle upon a Route

public class Obstacle : Tile
{
    [Header("Basic Obstacle Property")]
    [SerializeField] private Image _currHPImg = null;
    [SerializeField] private string _name = null;

    public float _HP;

    private float _currHP;

    private List<Monster> _monsters = new List<Monster>();
    private RaycastHit _hit;

    void Start()
    {
        _currHP = _HP;
        InvokeRepeating("UpdateEnemyList", 0.0f, 0.1f);
    }

    void Update()
    {

        if (_currHP <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    private void UpdateEnemyList()
    {
        Monster[] monsters = FindObjectsOfType<Monster>();
        foreach(Monster monster in monsters)
        {
            // 현재 위치와 다음 웨이브 포인트 사이에 장애물 위치하면 SetObstacle되도록
            if (Physics.Raycast(new Vector3(monster.transform.position.x, monster.transform.position.y, 0.5f),
                                  new Vector3(monster.GetNextPos().x, monster.GetNextPos().y, 0.5f), out _hit))
                Debug.Log("Get");

            if ((Vector2.Distance(monster.transform.position, gameObject.transform.position) <= 1.0f))
            {
                if (!_monsters.Contains(monster))
                {
                    _monsters.Add(monster);
                    monster.SetObstacle(this);
                }
            }
            else
            {
                if(_monsters.Contains(monster))
                {
                    _monsters.Remove(monster);
                }
            }
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
}

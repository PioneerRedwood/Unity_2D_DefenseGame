using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMoveMonster : Monster
{
    [SerializeField] private float _hp = 300;

    public override void OnDamage(float damage)
    {
        _hp -= damage;
        //Debug.Log(name + "   hp: " + _hp);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveToNext();
        if (_hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}

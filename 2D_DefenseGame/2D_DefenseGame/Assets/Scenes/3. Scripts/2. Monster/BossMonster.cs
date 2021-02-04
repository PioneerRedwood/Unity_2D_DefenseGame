using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 보스 몬스터:
 *      월등한 체력
 *      일정 시간마다 주변 타워 공격력 저하 오라 발생
 */

public class BossMonster : Monster
{
    [Header("BossMonster property")]
    [Range(0, 3)]
    [SerializeField] private float _auraRange = 2.0f;
    [Range(0, 1)]
    [SerializeField] private float _decreaseDamage = 0.0f;
    [SerializeField] private float _decreaseDuration = 0.0f;
    
    public override void OnDamage(float damage)
    {
        _currHP -= damage;
    }

    protected override void ShowHP()
    {
        _currHPImg.fillAmount = _currHP / _hp;
    }

    void Start()
    {
        _currHP = _hp;
        InvokeRepeating("ActivateAura", 1.0f, 1.0f);
    }

    void Update()
    {
        MoveToNext();
        ShowHP();
        if(_currHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    void ActivateAura()
    {
        Tower[] towers = GameObject.FindObjectsOfType<Tower>();
        
        foreach (Tower tempTower in towers)
        {
            if (Vector2.Distance(tempTower.transform.position, transform.position) <= _auraRange)
            {
                tempTower.DecreaseDamage(_decreaseDamage, _decreaseDuration);
            }
        }
    }
}

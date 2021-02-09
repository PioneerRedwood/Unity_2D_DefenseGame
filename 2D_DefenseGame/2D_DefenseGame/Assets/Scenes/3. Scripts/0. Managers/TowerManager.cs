using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [Header("Tower")]
    [SerializeField] public Tower[] Common;
    [SerializeField] public Tower[] Uncommon;
    [SerializeField] public Tower[] Rare;
    [SerializeField] public Tower[] Unique;
    [SerializeField] public Tower[] Legendary;

    private void Start()
    {
        Player.GetInstance().LoadTower(this);
    }
}

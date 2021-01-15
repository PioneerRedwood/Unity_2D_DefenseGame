using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    // 나올 몬스터 종류, 숫자
    [Serializable]
    public class WaveBundle
    {
        // prefab? class type?
        public Monster monster = null;
        public int numOfMonster = 0;
    }

    [SerializeField] private List<WaveBundle> bundles = null;

    public WaveBundle GetWaveBundle(int i)
    {
        if (bundles[i] != null)
        {
            return bundles[i];
        }
        return null;
    }

    public int GetNumOfWave()
    {
        return bundles.Count;
    }
}

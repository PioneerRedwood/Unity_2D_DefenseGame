using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttackRange : MonoBehaviour
{
    public float _imageScale;

    void Start()
    {
        OffAttackRange();
    }

    public void OnAttackRange(float range, Vector3 position)
    {
        gameObject.SetActive(true);

        float diameter = range;
        transform.localScale = Vector3.one * diameter* _imageScale;

        transform.position = position;
    }

    public void OffAttackRange()
    {
        gameObject.SetActive(false);
    }


}

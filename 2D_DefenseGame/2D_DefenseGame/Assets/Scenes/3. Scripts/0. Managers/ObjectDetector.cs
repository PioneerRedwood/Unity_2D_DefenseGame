using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField] private UIManager _UIManager = null;

    private Camera _main;
    private Ray _ray;
    private RaycastHit _hit;

    void Start()
    {
        _main = Camera.main;   
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _ray = _main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity))
            {

                if (_hit.transform.CompareTag("Ground"))
                {
                    _UIManager.GroundClicked(_hit.collider.gameObject);
                }
                else if(_hit.transform.CompareTag("Route"))
                {
                    _UIManager.RouteClicked(_hit.collider.gameObject);
                }
                else if(_hit.transform.CompareTag("Enemy"))
                {
                    _UIManager.MonsterClicked(_hit.collider.gameObject);
                }
                else if(_hit.transform.CompareTag("Obstacle"))
                {
                    _UIManager.RouteClicked(_hit.transform.parent.gameObject);
                }
            }
        }
    }
}

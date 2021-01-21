using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private UIManager _UIManager = null;

    private Camera _main;
    private Ray _ray;
    private RaycastHit _hit;

    // Start is called before the first frame update
    void Start()
    {
        _main = Camera.main;   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _ray = _main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity))
            {

                if (_hit.transform.CompareTag("Ground"))
                {
                    _UIManager.GroundClick(_hit.collider.gameObject);
                    Debug.Log(_hit.collider.gameObject.name);
                }
                else if(_hit.transform.CompareTag("Route"))
                {
                    _UIManager.RouteClick(_hit.collider.gameObject);
                    Debug.Log(_hit.collider.gameObject.name);
                }
                
            }
        }
    }
}

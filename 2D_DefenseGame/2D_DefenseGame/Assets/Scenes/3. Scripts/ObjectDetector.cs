﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private UIManager UIManager;

    private Camera Main;
    private Ray ray;
    private RaycastHit Hit;

    // Start is called before the first frame update
    void Start()
    {
        Main = Camera.main;   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            ray = Main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out Hit, Mathf.Infinity))
            {
                if (Hit.transform.CompareTag("Ground"))
                {
                    UIManager.GroundClick();
                    Debug.Log("groundclick!");
                }
                else if (Hit.transform.CompareTag("Route"))
                {
                    UIManager.RouteClick();
                    Debug.Log("click!");
                }
            }
            
        }
    }
}

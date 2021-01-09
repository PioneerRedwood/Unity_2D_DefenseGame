using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject UIPanel;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RouteClick()
    {
        UIPanel.SetActive(true);


    }

    public void GroundClick(){
        UIPanel.SetActive(false);

    }
 
}

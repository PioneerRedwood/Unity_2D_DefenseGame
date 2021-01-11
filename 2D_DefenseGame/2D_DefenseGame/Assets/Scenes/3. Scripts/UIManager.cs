using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject UIPanel;
    [SerializeField]
    private GameObject[] Buttons;

    private GameObject SelectedObj;

    private bool IsRoutePanel = false;
    private bool IsGroundPanel = false;

    private Ground Ground_componet;
    private Route Route_componet;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RouteClick(GameObject _Route)
    {
        if(SelectedObj == _Route)
        {
            resetPanel();
            SelectedObj = null;
            return;
        }
        SelectedObj = _Route;
        LoadPanel(SelectedObj);

    }

    public void GroundClick(GameObject _Ground)
    {
        if (SelectedObj == _Ground)
        {
            resetPanel();
            SelectedObj = null;
            return;
        }

        SelectedObj = _Ground;
        LoadPanel(SelectedObj);

    }

    #region Panel

    public void LoadPanel(GameObject SelectedObj)
    {
        if (SelectedObj.CompareTag("Route"))
        {
            Route_componet = SelectedObj.GetComponent<Route>();

            if (Ground_componet == null)
            {
                return;
            }
        }
        else if (SelectedObj.CompareTag("Ground"))
        {
            resetPanel();
            Ground_componet = SelectedObj.GetComponent<Ground>();

            if(Ground_componet == null)
            {
                return;
            }

            if(Ground_componet.IsBuildTower == false)
            {
                CreateButton(Buttons[0]);
            }
            else if(Ground_componet.IsBuildTower == true)
            {
                CreateButton(Buttons[1]);
                CreateButton(Buttons[2]);
            }
        }

    }

    public void CreateButton(GameObject Input)
    {
        GameObject Button = Instantiate(Input);

        Button.transform.position = UIPanel.transform.position;
        Button.transform.SetParent(UIPanel.transform);

    }

    public void resetPanel()
    {
        Transform child = UIPanel.GetComponentInChildren<Transform>();

        foreach (Transform iter in child)
        {
            // 부모(this.gameObject)는 삭제 하지 않기 위한 처리
            if (iter != UIPanel.transform)
            {
                Destroy(iter.gameObject);
            }
        }
    }

    #endregion

    #region Buttons

    public void BuildButton()
    {
        
    }

    #endregion

}

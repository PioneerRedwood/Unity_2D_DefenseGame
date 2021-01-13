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

    private Ground Ground_component;
    private Route Route_component;
    private TileOutline TileOutline_component;

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
        TileOutline_component = _Route.GetComponent<TileOutline>();

        if (SelectedObj == _Route)
        {
            resetPanel();
            TileOutline_component.OutlineDisable();
            SelectedObj = null;
            return;
        }

        TileOutline_component.OutlineEnable();

        SelectedObj = _Route;
        LoadPanel(SelectedObj);

    }

    public void GroundClick(GameObject _Ground)
    {
        TileOutline_component = _Ground.GetComponent<TileOutline>();

        if (SelectedObj == _Ground)
        {
            resetPanel();
            TileOutline_component.OutlineDisable();
            SelectedObj = null;
            return;
        }
        TileOutline_component.OutlineEnable();

        SelectedObj = _Ground;
        LoadPanel(SelectedObj);
    }

    #region Panel

    public void LoadPanel(GameObject SelectedObj)
    {
        resetPanel();

        if (SelectedObj.CompareTag("Route"))
        {
            Route_component = SelectedObj.GetComponent<Route>();

            if (Route_component == null)
            {
                return;
            }
        }
        else if (SelectedObj.CompareTag("Ground"))
        {
            Ground_component = SelectedObj.GetComponent<Ground>();

            if(Ground_component == null)
            {
                return;
            }

            if(Ground_component.IsBuildTower == false)
            {
                CreateButton(Buttons[0]);
            }
            else if(Ground_component.IsBuildTower == true)
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

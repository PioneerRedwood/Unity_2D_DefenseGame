using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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

    private int TowerPrice = 0;

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

    public void LoadPanel(GameObject SelectedObj) { 
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
                CreateButton(0);
            }
            else if(Ground_component.IsBuildTower == true)
            {
                CreateButton(1);
                CreateButton(2);
            }
        }
    }

    public void CreateButton(int num)
    {
     
        GameObject Button = Instantiate(Buttons[num]);

        Button.transform.position = UIPanel.transform.position;
        Button.transform.SetParent(UIPanel.transform);

        Button BtnListener = Button.GetComponent<Button>();

        switch (num)
        {
            case 0:
                BtnListener.onClick.AddListener(BuildButton);
                break;
            case 1:
                BtnListener.onClick.AddListener(MergeButton);
                break;
            case 2:
                BtnListener.onClick.AddListener(SellButton);
                break;
            default:
                break;
        }

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

    #region GroundButtons

    public void BuildButton()
    {

        if ((Player.getInstance().getMoney() - TowerPrice) < 0 ) {
            Debug.Log("Not Enough Minerals");
            return;
        }

        if (Ground_component.IsBuildTower)
        {
            return;
        }

        Ground_component.IsBuildTower = true;

        Tower _Tower = Instantiate(GamePrefabManager.Instance.towerPrefab, SelectedObj.transform.position, Quaternion.identity);

        _Tower.transform.position = Ground_component.transform.position;
        _Tower.transform.SetParent(Ground_component.transform.parent);

        Player.getInstance().AddTower(_Tower);
        LoadPanel(SelectedObj);
    }

    public void MergeButton()
    {
        Debug.Log("MERGE");

    }

    public void SellButton()
    {
        Debug.Log("SELL");
        if (Ground_component.IsBuildTower == false)
        {
            return;
        }

        Player.getInstance().DeleteTower(SelectedObj.transform.parent);

        Player.getInstance().addMoney(TowerPrice / 2);

        Ground_component.IsBuildTower = false;
        LoadPanel(SelectedObj);
    }



    #endregion

}

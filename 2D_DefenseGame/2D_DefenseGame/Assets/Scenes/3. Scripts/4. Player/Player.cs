using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player _instance;
    public static Player getInstance()
    {
        if (_instance == null)
        {
            _instance = new Player();
        }

        return _instance;
    }

    //public StageManager _StageManager;
    private bool _IsPlaying = false;
    private TowerManager TowerManager;
    private int Money = 0;
    private int Life = 0;

    private List<Tower> TowerList = new List<Tower>();

    // Start is called before the first frame update
    void Awake()
    {
        //_StageManager = GameObject.Find("StageManager").GetComponent<StageManager>();

        if(_instance == null)
        {
            _instance = this;
        }
 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIsNowPlayingFlag(bool IsPlaying)
    {
        _IsPlaying = IsPlaying;
    }

    public void ResetGame()
    {
        Money = 0;
        Life = 0;
        TowerList.Clear();
    }


    public void addMoney(int add)
    {
        Money += add;
    }
    public int getMoney()
    {
        return Money;
    }
    public void addLife(int add)
    {
        Life += add;
    }
    public int getLife()
    {
        return Life;
    }

    public List<Tower> getTowerList()
    {
        return TowerList;
    }
    public Tower getTower(Transform parent)
    {
        Tower find = new Tower();
        foreach (Tower tower in TowerList)
        {
            if (tower.transform.parent == parent)
            {
                find = tower;
                break;
            }
        }
        return find;
    }

    public void BuildTower(GameObject SelectedObj)
    {
        Tower _Tower = Instantiate(getRandomTower(TowerManager.Common), SelectedObj.transform.position, Quaternion.identity);

        _Tower.transform.position = SelectedObj.transform.position;
        _Tower.transform.SetParent(SelectedObj.transform.parent);

        AddTower(_Tower);
    }

    public void BuildTower(GameObject SelectedObj, Tower tower)
    {
        Tower _Tower = Instantiate(tower, SelectedObj.transform.position, Quaternion.identity);

        _Tower.transform.position = SelectedObj.transform.position;
        _Tower.transform.SetParent(SelectedObj.transform.parent);

        AddTower(_Tower);
    }

    public void DeleteTower(Transform Delete)
    {
        int towerindex = 0;

        foreach(Tower tower in TowerList)
        {

            if (tower.transform.parent == Delete)
            {
                Debug.Log("Deleted!");

                TowerList.RemoveAt(towerindex);
                Destroy(tower.gameObject);
                break;
            }

            towerindex++;
        }
    }

    public void MergeTower(GameObject SelectedObj)
    {
        Tower mergedTower;
        Tower SelectedTower = getTower(SelectedObj.transform.parent);

        foreach (Tower tower in TowerList)
        {
            if (tower.towerName == SelectedTower.towerName && tower != SelectedTower)
            {

                switch (SelectedTower.Tier.ToString())
                {
                    case "common":
                        BuildTower(SelectedObj, getRandomTower(TowerManager.Uncommon));
                        break;
                    case "uncommon":
                        BuildTower(SelectedObj, getRandomTower(TowerManager.Rare));
                        break;
                    case "rare":
                        BuildTower(SelectedObj, getRandomTower(TowerManager.Unique));
                        break;
                    case "unique":
                        BuildTower(SelectedObj, getRandomTower(TowerManager.Legendary));
                        break;
                    case "legendary":
                        break;
                    default:
                        break;
                }

                TowerList.Remove(SelectedTower);
                TowerList.Remove(tower);

                tower.transform.parent.GetChild(0).GetComponent<Ground>().IsBuildTower = false;

                Destroy(SelectedTower.gameObject);
                Destroy(tower.gameObject);

                break;
            }
        }
    }

    public void AddTower(Tower add)
    {
        if (add == null)
        {   
            return;
        }

        TowerList.Add(add);
    }

    public void LoadTower(TowerManager input)
    {
        TowerManager = input;
        if(TowerManager == null)
        {
            TowerManager = GameObject.FindObjectOfType<TowerManager>();
        }

    }

    private Tower getRandomTower(Tower[] List)
    {
        Tower RT = List[Random.Range(0, List.Length)];

        return RT;
    }

}

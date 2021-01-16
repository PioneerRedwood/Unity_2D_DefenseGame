using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player _instance;
    public static Player GetInstance()
    {
        if (_instance == null)
        {
            _instance = new Player();
        }

        return _instance;
    }

    private TowerManager _towerManager;
    private int _money = 0;
    private int _life = 0;

    private List<Tower> _towerList = new List<Tower>();

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    #region Property
    public void ResetGame()
    {
        _money = 0;
        _life = 0;
        _towerList.Clear();
    }
    public void addMoney(int num)
    {
        _money += num;
    }
    public int getMoney()
    {
        return _money;
    }
    public void addLife(int num)
    {
        _life += num;
    }
    public int getLife()
    {
        return _life;
    }
    #endregion

    #region Tower
    public List<Tower> GetTowerList()
    {
        return _towerList;
    }

    public Tower GetTower(Transform parent)
    {
        Tower find = new Tower();
        foreach (Tower tower in _towerList)
        {
            if (tower.transform.parent == parent)
            {
                find = tower;
                break;
            }
        }
        return find;
    }

    public void BuildTower(GameObject selectedObj)
    {
        Tower tempTower = Instantiate(GetRandomTower(_towerManager.Common), selectedObj.transform.position, Quaternion.identity);

        tempTower.transform.position = selectedObj.transform.position;
        tempTower.transform.SetParent(selectedObj.transform.parent);

        AddTower(tempTower);
    }

    public void BuildTower(GameObject selectedObj, Tower tower)
    {
        Tower _tempTower = Instantiate(tower, selectedObj.transform.position, Quaternion.identity);

        _tempTower.transform.position = selectedObj.transform.position;
        _tempTower.transform.SetParent(selectedObj.transform.parent);

        AddTower(_tempTower);
    }

    public void DeleteTower(Transform delete)
    {
        int towerindex = 0;

        foreach (Tower tower in _towerList)
        {

            if (tower.transform.parent == delete)
            {
                Debug.Log("Deleted!");

                _towerList.RemoveAt(towerindex);
                Destroy(tower.gameObject);
                break;
            }

            towerindex++;
        }
    }

    public void MergeTower(GameObject selectedObj)
    {
        Tower mergedTower;
        Tower SelectedTower = GetTower(selectedObj.transform.parent);

        foreach (Tower tower in _towerList)
        {
            if (tower._towerName == SelectedTower._towerName && tower != SelectedTower)
            {
                switch (SelectedTower._tier.ToString())
                {
                    case "Common":
                        BuildTower(selectedObj, GetRandomTower(_towerManager.Uncommon));
                        break;
                    case "Uncommon":
                        BuildTower(selectedObj, GetRandomTower(_towerManager.Rare));
                        break;
                    case "Rare":
                        BuildTower(selectedObj, GetRandomTower(_towerManager.Unique));
                        break;
                    case "Unique":
                        BuildTower(selectedObj, GetRandomTower(_towerManager.Legendary));
                        break;
                    case "Legendary":
                        break;
                    default:
                        break;
                }

                _towerList.Remove(SelectedTower);
                _towerList.Remove(tower);

                tower.transform.parent.GetChild(0).GetComponent<Ground>().IsBuildTower = false;

                Destroy(SelectedTower.gameObject);
                Destroy(tower.gameObject);

                break;
            }
        }
    }

    public void AddTower(Tower tower)
    {
        if (tower == null)
        {
            return;
        }

        _towerList.Add(tower);
    }

    public void LoadTower(TowerManager towerManager)
    {
        _towerManager = towerManager;
        if (_towerManager == null)
        {
            _towerManager = GameObject.FindObjectOfType<TowerManager>();
        }

    }

    private Tower GetRandomTower(Tower[] List)
    {
        Tower randomTower = List[Random.Range(0, List.Length)];

        return randomTower;
    }

    #endregion

}

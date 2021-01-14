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

    public void AddTower(Tower add)
    {
        if (add == null)
        {
            return;
        }

        TowerList.Add(add);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public StageManager _StageManager;
    public bool _IsPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        _StageManager = GameObject.Find("StageManager").GetComponent<StageManager>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BuildTower()
    {

    }

    public void SetIsNowPlayingFlag(bool IsPlaying)
    {
        _IsPlaying = IsPlaying;
    }
}

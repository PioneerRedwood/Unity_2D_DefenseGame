using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : Tile
{
    public bool IsBuildTower;

    // Start is called before the first frame update
    void Start()
    {
        IsBuildTower = false;
    }


}

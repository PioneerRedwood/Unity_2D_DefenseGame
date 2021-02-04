using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : Tile
{
    private bool _isObstacleBuilt = false;

    public void SetObstacleBuilt(bool isObstacleBuilt)
    {
        _isObstacleBuilt = isObstacleBuilt;
    }
    
    public bool GetObstacleBuilt()
    {
        return _isObstacleBuilt;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : Tile
{
    private bool _isTowerBuilt = false;

    public void SetTowerBuilt(bool isTowerBuilt)
    {
        _isTowerBuilt = isTowerBuilt;
    }

    public bool GetTowerBuilt()
    {
        return _isTowerBuilt;
    }
}

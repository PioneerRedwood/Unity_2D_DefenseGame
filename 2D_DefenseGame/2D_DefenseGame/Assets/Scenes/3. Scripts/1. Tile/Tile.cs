using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour
{
    public void AddTileComponentEditor<T>(T tileComponent) where T : Tile
    {
        // 다른 컴포넌트가 있으면 삭제하고 추가
        var otherComponent = GetComponentInChildren<T>();

        if(otherComponent != null)
        {
            DestroyImmediate(tileComponent.gameObject);
            DestroyImmediate(otherComponent.gameObject);
            return;
        }

        tileComponent.transform.parent = transform;
        tileComponent.transform.localPosition = Vector3.zero;
    }

    public void RemoveAllTileComponents()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    public void RemoveTileComponent<T>(T tileComponent) where T : Tile
    {
        var otherComponent = GetComponentInChildren<T>();
        if (otherComponent != null)
        {
            DestroyImmediate(tileComponent.gameObject);
            DestroyImmediate(otherComponent.gameObject);
            return;
        }
        DestroyImmediate(tileComponent.gameObject);
    }

    public void AddTileComponent(Tile tileComponent)
    {
        // 다른 컴포넌트가 위에 있더라도 추가
        tileComponent.transform.parent = transform;
        tileComponent.transform.localPosition = Vector3.zero;
    }

    public void OnClick()
    {
        Debug.Log("OnClick()  " + name);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 본 프로젝트에서 사용되지 않음
public class TileManager : MonoBehaviour
{
    public int SIZE;
    public Tile tilePrefab;
    public Vector3 CameraPosition;

    [SerializeField]
    private Tile[] tiles;

    GameObject cameraObject;
    public GameObject StageManagerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //stageManager.stages[0]

        // 카메라 포지션 바꾸기 - 실제 적용에선 어떻게 할 지 토의 필요
        cameraObject = GameObject.Find("Main Camera");
        cameraObject.transform.position = new Vector3(SIZE / 2, SIZE / 2, -5);
        //cameraObject.transform.position = CameraPosition;

        tiles = new Tile[SIZE * SIZE];
        //if (MakeTiles(SIZE))
        //{
        //    // 타일 만들기 실패하면 게임 종료
        //    Debug.Log("Successfully worked! MakeTiles(SIZE)");
        //}
        //else
        //{
        //    Debug.Log("Error at MakeTiles(SIZE)");
        //}
    }

    // Update is called once per frame
    void Update()
    {

    }

    //private bool MakeTiles(int size)
    //{
    //    for (int i = 0; i < size; i++)
    //    {
    //        for (int j = 0; j < size; j++)
    //        {
    //            /* 타일 생성
    //             * 타일 위치에 맞게 이름 짓기
    //             * 타일 매니저 하위에 추가
    //             * 
    //             */
    //            tiles[i * size + j] = (Tile)Instantiate(tilePrefab, new Vector3(i, j, 0), Quaternion.identity);
    //            tiles[i * size + j].name = "Tile[" + i + "][" + j + "]";
    //            tiles[i * size + j].transform.parent = this.transform;

    //            // 생성 못하면 없애기
    //            if (tiles[i * size + j] == null)
    //            {
    //                return false;
    //            }
    //        }
    //    }
    //    return true;
    //}
}



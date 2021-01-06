using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    private float hp = 0;
    private float speed = 0;
    enum State
    {
        moving, stop, dead, normal
    }
    private State state = State.normal;
    private Vector2Int[] Waypoint { get; set; }
    private Vector2Int currentPos;

    void Start()
    {

    }

    void Update()
    {
        // 끝 지점 도달은 Tile에서 몬스터로 알려주는 이벤트 발생으로
        // MoveToWaypoint()
        // 만약 끝 지점 플레이어 체력 깎고 자신 파괴
    }

    #region HP
    public float GetHP()
    {
        return hp;
    }

    public abstract bool OnDamage(float damage);

    public void DestroyMonster()
    {
        if (state != State.dead)
        {
            state = State.dead;
            Destroy(this);
        }
    }
    #endregion

    #region Waypoint
    public void InitWaypoint(Vector2Int[] waypoints)
    {
        Waypoint = waypoints;
    }
    #endregion

    // 이동 관련 함수 - 다음 이동할 위치가 남아있는지
    public bool IsRemainNextPoint(Vector2 currentPos)
    {
        return false;
    }


}

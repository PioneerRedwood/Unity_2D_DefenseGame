using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    enum MonsterState
    {
        moving, stop, dead, normal
    }
    [SerializeField] private float hp;
    [SerializeField] private float speed;

    private MonsterState state = MonsterState.normal;
    private Vector2Int[] Waypoint { get; set; }
    private int wayPointIdx = 0;
    private Vector2Int nextPos;

    public GameObject Player;

    void Start()
    {
        Player = GameObject.Find("Player");
    }

    void Update()
    {
        // 끝 지점 도달은 Tile에서 몬스터로 알려주는 이벤트 발생? -- 고려
        // MoveToWaypoint()
        // 만약 끝 지점 플레이어 체력 깎고 자신 파괴
        if (Waypoint != null)
            MoveToNext();

        if ((Vector2)transform.position == Waypoint[Waypoint.Length - 1])
        {
            Destroy(gameObject);
        }
    }

    #region Damage
    public abstract bool OnDamage(float damage);

    public void DestroyMonster()
    {
        if (state != MonsterState.dead)
        {
            state = MonsterState.dead;
            Destroy(this);
        }
    }
    #endregion

    #region Waypoint : Basic movement
    public void InitWaypoint(Vector2Int[] waypoints)
    {
        Waypoint = waypoints;
        nextPos = Waypoint[wayPointIdx + 1];
    }

    // 이동 유튜브 참고
    // https://www.youtube.com/watch?v=ExRQAEm4jPg&ab_channel=AlexanderZotov
    private void MoveToNext()
    {
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)nextPos, speed * Time.deltaTime);

        if (nextPos == Waypoint[wayPointIdx + 1])
        {
            nextPos = Waypoint[++wayPointIdx];
        }
    }
    #endregion

}

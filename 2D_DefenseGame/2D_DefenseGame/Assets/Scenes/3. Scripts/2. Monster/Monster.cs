using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Monster : MonoBehaviour
{
    [Header("Monster Property")]
    public GameObject _hpTextPref = null;

    protected Vector2Int[] _waypoints { get; set; }
    protected Vector2Int _nextPos;
    protected int _wayPointIdx = 0;
    
    #region Damage
    public abstract void OnDamage(float damage);


    // HP 전시하려면 월드 캔버스에 몬스터 수만큼의 텍스트를 만들고 체력이 바뀔 때마다 업데이트를 해야함
    // 오류남 일단 주석처리
    public void SetHPText(GameObject textPref)
    {
        //_hpTextPref = textPref;
    }

    protected void ShowHP(float hp)
    {
        //Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        //_hpTextPref.GetComponent<RectTransform>().transform.position = (Vector3)screenPos + new Vector3(0f, -2.0f, 0f);
        ////_hpTextPref.transform.position = (Vector3)transform.position;
        //_hpTextPref.GetComponentInChildren<Slider>().value = hp;
    }

    protected void DestroyMonster()
    {
        Destroy(gameObject);
        Destroy(_hpTextPref);

    }
    #endregion

    #region Waypoint movement
    public void InitWaypoint(Vector2Int[] waypoints)
    {
        _waypoints = waypoints;
        _nextPos = _waypoints[_wayPointIdx + 1];
    }

    // 이동 유튜브 참고
    // https://www.youtube.com/watch?v=ExRQAEm4jPg&ab_channel=AlexanderZotov
    protected void MoveToNext(float speed)
    {
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)_nextPos, speed * Time.deltaTime);

        if ((Vector2)transform.position == _nextPos && _nextPos != _waypoints[_waypoints.Length - 1])
        {
            _nextPos = _waypoints[++_wayPointIdx];
        }

        if ((Vector2)transform.position == _waypoints[_waypoints.Length - 1])
        {
            Destroy(gameObject);
            Player.GetInstance().LoseLife(1);
        }
    }
    #endregion
}

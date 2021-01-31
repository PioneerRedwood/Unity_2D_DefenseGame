using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] private Image _sprite = null;
    [SerializeField] private Text _name = null;
    [SerializeField] private Text _damage = null;
    [SerializeField] private Text _tier = null;
    [SerializeField] private Text _range = null;

    public void OnPanel(Transform tower)
    {
        gameObject.SetActive(true);

        Tower _towerInfo = tower.GetComponent<Tower>();

        _sprite.sprite = _towerInfo.GetComponent<SpriteRenderer>().sprite;
        _name.text = "이름: " + _towerInfo._towerName.ToString();
        _damage.text = "공격력: " + _towerInfo._damage.ToString();
        _tier.text = "등급: " + _towerInfo._tier.ToString();
        _range.text = "공격범위: " + _towerInfo._range.ToString();
    }

    public void OnPanel(Monster monster)
    {
        gameObject.SetActive(true);

        _sprite.sprite = monster.GetComponent<SpriteRenderer>().sprite;

        _name.text = "이름: " + monster.name.ToString();
        _damage.text = "체력: " + monster.GetCurrHP();
        _tier.text = "이동속도: " + monster.GetcurrSpeed();
        _range.text = "" ;
    }

    public void OffPanel()
    {
        gameObject.SetActive(false);
    }
}

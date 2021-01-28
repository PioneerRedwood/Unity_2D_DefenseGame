using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] private Image _towerSprite = null;
    [SerializeField] private Text _towerDamage = null;
    [SerializeField] private Text _towerTier = null;
    [SerializeField] private Text _towerRange = null;

    public void OnPanel(Transform tower)
    {
        gameObject.SetActive(true);

        Tower _towerinfo = tower.GetComponent<Tower>();

        _towerSprite.sprite = _towerinfo.GetComponent<SpriteRenderer>().sprite;
        _towerDamage.text = "공격력: " + _towerinfo._damage.ToString();
        _towerTier.text = "등급: " + _towerinfo._tier.ToString();
        _towerRange.text = "공격범위: " + _towerinfo._range.ToString();

    }

    public void OffPanel()
    {
        gameObject.SetActive(false);
    }

}

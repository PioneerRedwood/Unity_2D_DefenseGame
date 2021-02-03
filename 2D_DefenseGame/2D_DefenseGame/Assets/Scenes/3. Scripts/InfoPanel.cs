using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] private Image _profileSprite = null;
    [SerializeField] private Text _nameText = null;
    [SerializeField] private Text _damageText = null;
    [SerializeField] private Text _tierText = null;
    [SerializeField] private Text _rangeText = null;

    [SerializeField] private Image _stateImage1 = null;
    [SerializeField] private Image _stateImage2 = null;

    public void OnTowerPanel(Tower tower)
    {
        gameObject.SetActive(true);
        if(tower != null)
        {
            _profileSprite.sprite = tower.GetComponent<SpriteRenderer>().sprite;

            _nameText.text = "이름: " + tower._towerName;
            _damageText.text = "공격력: " + tower._damage.ToString();
            _tierText.text = "등급: " + tower._tier.ToString();
            _rangeText.text = "공격범위: " + tower._range.ToString();

            List<float> stateList = tower.GetState();

            if (stateList[0] != 0.0f)
            {
                _stateImage1.enabled = true;
                _stateImage1.GetComponent<RectTransform>().anchoredPosition = new Vector2(75, 20);
            }
            else
            {
                _stateImage1.enabled = false;
            }

            if (stateList[1] != 0.0f)
            {
                _stateImage2.enabled = true;
                _stateImage2.GetComponent<RectTransform>().anchoredPosition = new Vector2(125, 20);
            }
            else
            {
                _stateImage2.enabled = false;
            }

            if (!_stateImage1.enabled)
            {
                _stateImage2.GetComponent<RectTransform>().anchoredPosition = new Vector2(75, 20);
            }
        }
    }

    public void OnMonsterPanel(Monster monster)
    {
        gameObject.SetActive(true);
        if(monster != null)
        {
            _profileSprite.sprite = monster.GetComponentInChildren<SpriteRenderer>().sprite;

            _nameText.text = "이름: " + monster._monsterName.ToString();
            _damageText.text = "체력: " + monster.GetCurrHP().ToString();
            _tierText.text = "이동속도: " + monster.GetcurrSpeed().ToString();
            _rangeText.text = "";

            if (monster.GetState() != 0.0f)
            {
                _stateImage2.enabled = true;
                _stateImage1.enabled = false;
                _stateImage2.GetComponent<RectTransform>().anchoredPosition = new Vector2(75, 20);
            }
        }
    }

    public void OffPanel()
    {
        gameObject.SetActive(false);
        
        _stateImage1.enabled = false;
        _stateImage2.enabled = false;
    }
}

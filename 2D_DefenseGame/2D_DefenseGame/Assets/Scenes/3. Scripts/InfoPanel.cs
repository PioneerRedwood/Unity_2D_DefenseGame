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

            _nameText.text = "Name: " + tower._towerName;
            
            if(tower.GetComponent<BuffTower>() == null)
            {
                _damageText.text = "Damage: " + tower._damage.ToString();
            }
            else
            {
                _damageText.text = "Add damage: " + tower.GetComponent<BuffTower>().GetBuffInfo() * 100 + "%";
            }

            _tierText.text = "Tier: " + tower._tier.ToString();
            _rangeText.text = "Range: " + tower._range.ToString();

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
        else
        {
            OffPanel();
        }
    }

    public void OnMonsterPanel(Monster monster)
    {
        gameObject.SetActive(true);
        if(monster != null)
        {
            _profileSprite.sprite = monster.GetComponentInChildren<SpriteRenderer>().sprite;

            _nameText.text = "Name: " + monster.GetName().ToString();
            _damageText.text = "HP: " + monster.GetCurrHP().ToString();
            _tierText.text = "Speed: " + monster.GetCurrSpeed().ToString();
            _rangeText.text = "";

            if (monster.GetState() != 0.0f)
            {
                _stateImage2.enabled = true;
                _stateImage1.enabled = false;
                _stateImage2.GetComponent<RectTransform>().anchoredPosition = new Vector2(75, 20);
            }
        }
        else
        {
            OffPanel();
        }
    }

    public void OnObstaclePanel(Obstacle obstacle)
    {
        gameObject.SetActive(true);
        
        if(obstacle != null)
        {
            _profileSprite.sprite = obstacle.GetComponentInChildren<SpriteRenderer>().sprite;

            _nameText.text = "Name: " + obstacle.GetName();
            _damageText.text = "HP: " + obstacle.GetCurrHP().ToString();
            _tierText.text = obstacle.GetMonsterCount().ToString() + " Attached";
            _rangeText.text = "";
        }
        else
        {
            OffPanel();
        }
    }

    public void OffPanel()
    {
        gameObject.SetActive(false);
        
        _stateImage1.enabled = false;
        _stateImage2.enabled = false;
    }
}

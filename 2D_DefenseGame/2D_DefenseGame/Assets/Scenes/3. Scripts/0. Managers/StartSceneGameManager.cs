using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneGameManager : MonoBehaviour
{
    public void OnPlayClicked()
    {
        SceneManager.LoadScene("StageSelectScene");
    }

    public void OnSettingsClicked()
    {
        Debug.Log("Settings Clicked");
    }

    public void OnCreditsClicked()
    {
        Debug.Log("Credits Clicked");
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }

    public void InitGame()
    {
        if (PlayerPrefs.GetInt("ID_Play") == 0)
        {
            PlayerPrefs.SetString("StageClearCount", "1:0\t2:-1\t3:-1\t4:-1\t5:-1");
        }
        PlayerPrefs.SetInt("ID_Play", PlayerPrefs.GetInt("ID_Play") + 1);
        PlayerPrefs.Save();
    }

    private void Awake()
    {
        InitGame();
    }
}

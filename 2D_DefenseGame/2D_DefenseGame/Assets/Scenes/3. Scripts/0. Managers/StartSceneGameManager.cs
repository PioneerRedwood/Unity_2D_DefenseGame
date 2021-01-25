using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneGameManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

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
        Application.Quit(0);
    }
}

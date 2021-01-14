using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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

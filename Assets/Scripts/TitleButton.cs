using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;

public class TitleButton : MonoBehaviour
{
    
    public string nextSceneName;

    void Awake()
    {
        Time.timeScale = 1f;
    }

    public void StartClick()
    {
        LoadingSceneManager.LoadScene(nextSceneName);
        PlayerPrefs.SetInt("NewGame", 1);
    }

    public void LoadClick()
    {
        LoadingSceneManager.LoadScene(nextSceneName);
        PlayerPrefs.SetInt("NewGame", 0);
    }

    public void ExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

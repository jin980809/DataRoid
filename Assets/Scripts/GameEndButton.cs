using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndButton : MonoBehaviour
{
    public string startSceneName;

    public void RestartClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

    public void StartScene()
    {
        SceneManager.LoadScene(startSceneName);
    }

}

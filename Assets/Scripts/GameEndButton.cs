using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndButton : MonoBehaviour
{
    public string startSceneName;

    public void RestartClick()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("NewGame", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  
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
        Time.timeScale = 1f;
        SceneManager.LoadScene(startSceneName);
    }

    public void Resume()
    {
        GameManager.Instance.player.isSettingOn = false;
        UIManager.Instance.mainUIAnim.SetTrigger("Open_Main");
        Time.timeScale = 1;
    }
}

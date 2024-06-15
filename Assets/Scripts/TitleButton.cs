using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;

public class TitleButton : FadeController
{
    public float deleyTime;
    public string nextSceneName;
    public GameObject effect;

    private void Start()
    {
        fadeImage.color = new Color(0, 0, 0, 0);
        SoundManager.Instance.PlaySound2D("Start_Background");
    }

    void Awake()
    {
        Time.timeScale = 1f;
    }

    public void StartClick()
    {
        StartCoroutine(GameStart(1));
        effect.SetActive(true);
        FadeInOut();
    }

    public void LoadClick()
    {
        StartCoroutine(GameStart(0));
        effect.SetActive(true);
        FadeInOut();
    }

    IEnumerator GameStart(int newGameInt)
    {
        yield return new WaitForSeconds(deleyTime);
        LoadingSceneManager.LoadScene(nextSceneName);
        PlayerPrefs.SetInt("NewGame", newGameInt);
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

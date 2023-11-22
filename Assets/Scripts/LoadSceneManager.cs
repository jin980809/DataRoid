using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    bool rsDown;

    void Update()
    {
        rsDown = Input.GetButtonDown("ReStart");
        ReStartScene();
    }

    void ReStartScene()
    {
        if(rsDown)
        {
            SceneManager.LoadScene(0);
        }
    }
}

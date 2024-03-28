using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject UI;
    float timer;

    void Start()
    {
        timer += Time.deltaTime;

        if (timer > 6.5)
        {
            UI.SetActive(true);
        }
    }
}

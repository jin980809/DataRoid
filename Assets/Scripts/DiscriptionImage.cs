using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscriptionImage : MonoBehaviour
{

    public Player player;
    bool qDown;

    void Start()
    {
    }

    void Update()
    {
        qDown = Input.GetButtonDown("Cancel");


        if(qDown)
        {
            ExitImage();
        }
    }


    public void ExitImage()
    {
        player.isCommunicate = false;
        //transform.gameObject.SetActive(false);
        Time.timeScale = 1f;
        GetComponent<Animator>().SetTrigger("Guide_End");
    }
}

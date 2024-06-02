using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PassWord : MonoBehaviour
{
    public Interaction interaction;
    public bool isDone = false;

    public Player player;
    public bool isDroneOn;
    public bool isDeviceOn;
    public bool isNameTagOn;
    public int nameTagIndex;

    [Space(10)]
    [Header("Active")]
    public Collider[] a_col;
    public GameObject[] a_gameObj;
    public Interaction[] a_interaction;

    [Space(10)]
    [Header("Disable")]
    public Collider[] d_col;
    public GameObject[] d_gameObj;
    public Interaction[] d_interaction;

    void Start()
    {

    }

    public void NameTagOnOff()
    {
        if(isNameTagOn)
        {
            ObjectManager.Instance.SetNameTag(nameTagIndex);
        }
    }

    public void PassWordResult()
    {
        for (int i = 0; i < a_col.Length; i++)
            a_col[i].enabled = true;

        for (int i = 0; i < a_gameObj.Length; i++)
            a_gameObj[i].SetActive(true);

        for (int i = 0; i < a_interaction.Length; i++)
            a_interaction[i].enabled = true;

        for (int i = 0; i < d_col.Length; i++)
            d_col[i].enabled = false;

        for (int i = 0; i < d_gameObj.Length; i++)
            d_gameObj[i].SetActive(false);

        for (int i = 0; i < d_interaction.Length; i++)
            d_interaction[i].enabled = false;

        if (isDroneOn)
        {
            ObjectManager.Instance.saveObjects[0] = false;
            player.droneOn = true;
        }

        if(isDeviceOn)
        {
            ObjectManager.Instance.saveObjects[1] = false;
            UIManager.Instance.mainUI.SetActive(true);
            player.deviceOn = true;
        }
    }

    public void ExitPassWord()
    {
        interaction.isActive = false;
        interaction.v_Cam.SetActive(true);
        interaction.p_cameraMove.enabled = true;
        player.isCommunicate = false;
        StartCoroutine(ActiveFalse());
    }

    IEnumerator ActiveFalse()
    {
        yield return new WaitForSeconds(0.1f);
        transform.gameObject.SetActive(false);
    }
}

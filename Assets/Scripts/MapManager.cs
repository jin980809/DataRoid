using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<MapManager>();
            }

            return m_instance;
        }
    }
    private static MapManager m_instance;


    [Space(10)]
    [Header("Tut Map Data")]
    public bool isOfficeMapOpen;
    public bool isTutMapOpen;
    public GameObject officeMapObject;
    public Collider tutLightInteraction;
    public GameObject tutLightObject;
    public GameObject officeMapUI;
    public GameObject tutMapUI;
    public GameObject lockMapUI;

    [Space(10)]
    [Header("else")]
    public GameObject[] mapData;

    void Update()
    {
        if (!officeMapObject.activeSelf)
        {
            lockMapUI.SetActive(false);
            isOfficeMapOpen = true;
            officeMapUI.SetActive(true);
            tutLightInteraction.enabled = true;
        }

        if (!tutLightObject.activeSelf)
        {
            isTutMapOpen = true;
            tutMapUI.SetActive(true);
        }
    }

    public bool MapOpenCheck(GameObject[] gObj)
    {
        for(int i = 0; i < gObj.Length; i++)
        {
            if(gObj[i].activeSelf)
            {
                return false;
            }
        }

        return true;
    }

}

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
    public GameObject[] tutMapData;
    public bool isTutMapOpen;
    public Collider tutLightInteraction;

    [Space(10)]
    [Header("else")]
    public GameObject[] mapData;

    void Update()
    {
        isTutMapOpen = MapOpenCheck(tutMapData);
        tutLightInteraction.enabled = isTutMapOpen;
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

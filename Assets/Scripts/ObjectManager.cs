using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<ObjectManager>();
            }

            return m_instance;
        }
    }
    private static ObjectManager m_instance;

    [Header("ESN08")]
    public GameObject p1ServerRacks;
    public GameObject p2ServerRacks;
    public EnemyB ESN08;

    void Start()
    {

    }

    void Update()
    {
        ESN08Clear();
    }

    void ESN08Clear()
    {
        if (ESN08.isDeath)
        {
            p1ServerRacks.SetActive(false);
            p2ServerRacks.SetActive(true);
        }
        else
        {
            p1ServerRacks.SetActive(true);
            p2ServerRacks.SetActive(false);
        }
    }
}

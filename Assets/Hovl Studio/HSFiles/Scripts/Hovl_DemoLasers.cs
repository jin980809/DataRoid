using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using System;
using UnityEngine;

public class Hovl_DemoLasers : MonoBehaviour
{
    public GameObject FirePoint;
    public float MaxLength;
    public Transform target;
    public GameObject Prefabs;

    private GameObject Instance;
    private Hovl_Laser LaserScript;
    private Hovl_Laser2 LaserScript2;

    void Update()
    {
    }

    public void LazerOn()
    {
        //Destroy(Instance);
        Instance = Instantiate(Prefabs, FirePoint.transform.position, FirePoint.transform.rotation, transform.parent);
        //Instance.transform.parent = transform;
        LaserScript = Instance.GetComponent<Hovl_Laser>();
        LaserScript2 = Instance.GetComponent<Hovl_Laser2>();
    }


    public void LazerOff()
    {
        if (LaserScript) LaserScript.DisablePrepare();
        if (LaserScript2) LaserScript2.DisablePrepare();
        Destroy(Instance, 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricLine : MonoBehaviour
{
    public bool detectElec;
    //private bool alreadyDetect;
    public bool isDetectObjectOnOff;
    public GameObject[] detectOnObject;
    public GameObject[] detectOffObject;

    public bool isDontMove;
    public GameObject dataPath;

    public  ElectricLine e = null;

    void Update()
    {
        if(isDetectObjectOnOff)
        {
            if(detectElec)
            {
                for(int i = 0; i < detectOnObject.Length; i++)
                {
                    detectOnObject[i].SetActive(true);
                }
                for (int i = 0; i < detectOffObject.Length; i++)
                {
                    detectOffObject[i].SetActive(false);
                }
            }
            else
            {
                for (int i = 0; i < detectOnObject.Length; i++)
                {
                    detectOnObject[i].SetActive(false);
                }
                for (int i = 0; i < detectOffObject.Length; i++)
                {
                    detectOffObject[i].SetActive(true);
                }
            }
        }

        if (isDontMove)
        {
            /*if (detectElec)
            {
                dataPath.SetActive(true);
            }
            else
            {
                dataPath.SetActive(false);
            }*/
        }

        if (e != null)
        {
            if(!e.detectElec)
            {
                detectElec = false;
                e = null;
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Electric"))
        {
            ElectricLine elec = other.GetComponent<SetElecLine>().elec;
            if (elec.detectElec)
            {
                detectElec = true;
                e = elec;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Electric"))
        {
            if (this != other.GetComponent<SetElecLine>().elec.e)
            {
                //Debug.Log("AA");
                detectElec = false;
                e = null;
            }
        }
    }
}

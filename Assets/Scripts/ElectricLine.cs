using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricLine : MonoBehaviour
{

    public bool detectElec;
    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Electric"))
        {
            ElectricLine elec = other.GetComponent<SetElecLine>().elec;
            if (elec.detectElec)
            {
                detectElec = true;
            }
            else
            {
                detectElec = false;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Electric"))
        {
            detectElec = false;
        }
    }
}

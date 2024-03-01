using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBoxOpenTrigger : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //other.gameObject.transform.position = movePos.position;
        }
    }
}

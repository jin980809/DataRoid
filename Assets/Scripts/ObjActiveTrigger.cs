using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjActiveTrigger : MonoBehaviour
{
    public GameObject obj;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            obj.SetActive(true);
            GetComponent<Collider>().enabled = false;
        }
    }
}

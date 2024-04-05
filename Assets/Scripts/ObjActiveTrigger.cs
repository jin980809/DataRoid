using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjActiveTrigger : MonoBehaviour
{
    public GameObject obj;

    public bool isSave;
    public int objectID;

    void Start()
    {
        if (isSave)
        {
            transform.gameObject.SetActive(ObjectManager.Instance.saveObjects[objectID]);
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            obj.SetActive(true);
            GetComponent<BoxCollider>().enabled = false;

            if (isSave)
            {
                ObjectManager.Instance.saveObjects[objectID] = false;
            }

            //gameObject.SetActive(false);
        }
    }
}

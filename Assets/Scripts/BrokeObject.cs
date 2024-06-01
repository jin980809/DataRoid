using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokeObject : MonoBehaviour
{
    public int saveID;

    void Start()
    {
        transform.gameObject.SetActive(ObjectManager.Instance.saveObjects[saveID]);
    }

    public void SaveObject()
    {
        ObjectManager.Instance.saveObjects[saveID] = false;
    }
}

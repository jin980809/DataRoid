using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float curDamage;
    public float damage;
    public bool isExplosion;
    void Start()
    {
        curDamage = damage;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BrokenObject") && isExplosion)
        {
            other.GetComponent<BrokeObject>().SaveObject();
            other.gameObject.SetActive(false);
        }
    }
}

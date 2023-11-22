using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMarterial : MonoBehaviour
{
    public int getMarteria;
    public enum Type
    {
        Steel = 1,
        GunPowder = 2
    };
    public Type MarterialType;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch(MarterialType)
            {
                case Type.Steel:
                    MaterialManager.Instance.Steel += getMarteria;
                    break;

                case Type.GunPowder:
                    MaterialManager.Instance.GunPowder += getMarteria;
                    break;
            }
            
            Destroy(this.gameObject);
        }
    }
}

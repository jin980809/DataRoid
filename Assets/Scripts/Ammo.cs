using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public int getAmmo;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            MaterialManager.Instance.Ammo += getAmmo;
            Destroy(this.gameObject);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBDisapper : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }
}

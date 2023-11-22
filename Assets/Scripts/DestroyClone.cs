using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyClone : MonoBehaviour
{
    public float DeleteTime;

    void Start()
    {
        Destroy(this.gameObject, DeleteTime);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1f;
    public Vector3 direction = Vector3.forward;

    void Start()
    { 
    }
    void Update()
    {
        Move();
    }

    void Move()
    {
        float distance = speed * Time.deltaTime;

        transform.Translate(direction * distance);
    }
}

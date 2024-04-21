using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1f;
    public Vector3 direction = Vector3.forward;

    void Start()
    {
        Destroy(this, 0.4f);
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

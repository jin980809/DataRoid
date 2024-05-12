using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public Player player;
    public Transform targetPosition;
    public float followSpeed = 5f;
    public Transform playerPos;
    public float hoverHeight;
    float hoverOffset;
    float hoverSpeed = 3;

    void Start()
    {
        if (ObjectManager.Instance.saveObjects[0] == false)
        {
            transform.gameObject.SetActive(true);
            player.droneOn = true;

        }
        else
        {
            transform.gameObject.SetActive(false);
            player.droneOn = false;
        }
    }

    void Update()
    {
        hoverOffset = Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;

        transform.position = Vector3.Lerp(transform.position, targetPosition.position, followSpeed * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, new Vector3(targetPosition.position.x, targetPosition.position.y + hoverOffset, targetPosition.position.z), followSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, playerPos.rotation, followSpeed * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineEvent : FadeController
{

    [Space(10)]
    [Header("Player transform")]
    public GameObject player;
    public Transform spawnPoint;

    public void TransformPlayer()
    {
        player.transform.position = spawnPoint.position;
    }
}

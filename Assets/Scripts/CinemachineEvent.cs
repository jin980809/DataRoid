using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineEvent : FadeController
{

    [Space(10)]
    [Header("Player transform")]
    public GameObject gPlayer;
    public Transform spawnPoint;
    Player player;

    void Start()
    {
        player = gPlayer.GetComponent<Player>();
    }

    public void TransformPlayer()
    {
        gPlayer.transform.position = spawnPoint.position;
    }

    public void DontPlayerMove()
    {
        player.isCommunicate = true;
    }

    public void PlayerMove()
    {
        player.isCommunicate = false;
    }
}

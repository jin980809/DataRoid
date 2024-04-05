using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyASubdue : MonoBehaviour
{
    EnemyA enemyA;
    public Player player;
    private void Start()
    {
        enemyA = GetComponentInParent<EnemyA>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyA.anim.SetTrigger("doSubdue");
            enemyA.isPlayerSubdue = true;
            player.isSubdue = true;
        }
    }
}

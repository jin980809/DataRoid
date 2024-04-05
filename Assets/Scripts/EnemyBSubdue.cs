using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBSubdue : MonoBehaviour
{
    EnemyB enemyB;
    public Player player;
    private void Start()
    {
        enemyB = GetComponentInParent<EnemyB>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !player.isSubdue)
        {
            enemyB.anim.SetTrigger("doSubdue");
            enemyB.isPlayerSubdue = true;
            player.isSubdue = true;
        }
    }
}

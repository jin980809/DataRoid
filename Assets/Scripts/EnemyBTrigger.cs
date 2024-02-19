using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBTrigger : MonoBehaviour
{
    public GameObject enemy;
    public Player player;
    public int eventNum;
    public Transform enemySpawnPoint;
    public Transform enemyDisapperPoint;
    bool isSpawn = false;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && enemy != null && isSpawn == false)
        {
            enemy.SetActive(true);

            if (enemyDisapperPoint != null)
                enemy.GetComponent<NavMeshAgent>().SetDestination(enemyDisapperPoint.position);
        }
    }
}

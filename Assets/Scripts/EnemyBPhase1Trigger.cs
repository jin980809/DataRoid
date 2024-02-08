using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBPhase1Trigger : MonoBehaviour
{
    public GameObject enemy;
    public Player player;
    public int eventNum;
    public Transform enemySpawnPoint;
    public Transform enemyDisapperPoint;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && enemy != null)
        {
            enemy.SetActive(true);

            enemy.GetComponent<NavMeshAgent>().SetDestination(enemyDisapperPoint.position);
        }
    }
}

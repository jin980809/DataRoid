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
    //[SerializeField] bool isSpawn = false;
    public GameObject movePhase2;
    public EnemyB p2ESN08;

    void Start()
    {
        if(p2ESN08.isDeath)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && enemy != null)
        {
            enemy.SetActive(true);

            if (enemyDisapperPoint != null)
                enemy.GetComponent<NavMeshAgent>().SetDestination(enemyDisapperPoint.position);

            if (eventNum == 2)
                movePhase2.SetActive(true);
        }
    }
}

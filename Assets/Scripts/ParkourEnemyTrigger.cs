using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourEnemyTrigger : MonoBehaviour
{
    public GameObject player;
    public Transform spawnPoint;
    public GameObject parkourEnemyPreb;
    public GameObject enemyPreb;
    private GameObject parkourEnemy;
    private GameObject enemy;
    private BoxCollider boxCollider;
    private Animator enemyAnim;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if (enemyAnim != null)
        {
            if(enemyAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                StartCoroutine(SpawnEnemy());
                enemyAnim = null;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            parkourEnemy = Instantiate(parkourEnemyPreb, spawnPoint.position, spawnPoint.rotation, transform);
            boxCollider.enabled = false;
            enemyAnim = parkourEnemy.GetComponent<Animator>();
        }
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(0.2f);
        enemy = Instantiate(enemyPreb, parkourEnemy.transform.position, parkourEnemy.transform.rotation, transform);
        enemy.GetComponent<Enemy_MonsterWave>().target = player;
        Destroy(parkourEnemy);
    }
}

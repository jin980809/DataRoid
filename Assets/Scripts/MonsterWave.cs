using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterWave : MonoBehaviour
{
    public Player player;

    public Transform[] spawnPoints;
    public int[] spawnAmount;
    public GameObject prefabEnemy;
    public float spawnCool;
    private bool isSpawnStart = false;
    private Collider collider;

    void Start()
    {
        collider = GetComponent<BoxCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !isSpawnStart)
        {
            collider.enabled = false;

            for(int i = 0; i < spawnPoints.Length; i++)
            {
                StartCoroutine(SpawnEnemy(spawnPoints[i], spawnAmount[i]));
            }

            isSpawnStart = true;
        }
    }

    IEnumerator SpawnEnemy(Transform pos, int amount)
    {
        for(int i = 0; i < amount; i++)
        {        
            GameObject instEnemy = Instantiate(prefabEnemy, pos);

            Enemy enemy = instEnemy.GetComponent<Enemy>();
            enemy.target = player.gameObject;

            //instEnemy.transform.position = pos.position;

            yield return new WaitForSeconds(spawnCool);
        }

        isSpawnStart = false;
    }
}

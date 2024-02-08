using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();
            }

            return m_instance;
        }
    }
    private static GameManager m_instance;


    public Player player;
    public string fileName = "Save";

    public Transform[] spawnPoints;
    public int spawnPoint;

    public int ESN08Phase2Diff = 0;

    List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();

    void Start()
    {
        dicList.Clear();

        dicList = CSVReader.Read(fileName);

        ProgressManager.Instance.curProgress = int.Parse(dicList[0]["Progress"] + "");
        ProgressManager.Instance.saveProgress = int.Parse(dicList[0]["MaxProgress"] + "");
        MaterialManager.Instance.Ammo = int.Parse(dicList[0]["Ammo"] + "");
        MaterialManager.Instance.SpecialAmmo = int.Parse(dicList[0]["SpecialAmmo"] + "");
        MaterialManager.Instance.Steel = int.Parse(dicList[0]["Steel"] + "");
        MaterialManager.Instance.GunPowder = int.Parse(dicList[0]["GunPowder"] + "");

        spawnPoint = int.Parse(dicList[0]["SavePoint"] + "");

        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        player.transform.position = spawnPoints[spawnPoint].position;
    }

    void PlayerDead()
    {

    }

    void Update()
    {

    }
}

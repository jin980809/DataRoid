using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

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

    public string fileName = "Save.csv";
    List<string[]> data = new List<string[]>();
    string[] tempData;
    public Player player;
    public string wfileName = "Save";

    [Space(10)]
    [Header("Player Spawn")]
    public Transform[] spawnPoints;
    public int spawnPoint;


    [Space(10)]
    [Header("ESN08")]
    public int ESN08Phase2Diff = 0;

    List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();

    public bool isPlayerDead = false;

    public CameraMove cameraArm;
    void Awake()
    {
        data.Clear();

        tempData = new string[8];
        tempData[0] = "SavePoint";
        tempData[1] = "Data";
        tempData[2] = "MaxData";
        tempData[3] = "HandgunAmmo";
        tempData[4] = "RifleAmmo";
        tempData[5] = "ShotgunAmmo";
        tempData[6] = "LazerAmmo";
        tempData[7] = "UFSData";
        data.Add(tempData);
    }

    void Start()
    {
        dicList.Clear();

        dicList = CSVReader.Read(fileName);

        ProgressManager.Instance.curData = int.Parse(dicList[0]["Data"] + "");
        ProgressManager.Instance.saveData = int.Parse(dicList[0]["MaxData"] + "");
        MaterialManager.Instance.HandgunAmmo = int.Parse(dicList[0]["HandgunAmmo"] + "");
        MaterialManager.Instance.RifleAmmo = int.Parse(dicList[0]["RifleAmmo"] + "");
        MaterialManager.Instance.ShotgunAmmo = int.Parse(dicList[0]["ShotgunAmmo"] + "");
        MaterialManager.Instance.LazerAmmo = int.Parse(dicList[0]["LazerAmmo"] + "");
        MaterialManager.Instance.UFSData = int.Parse(dicList[0]["UFSData"] + "");

        spawnPoint = int.Parse(dicList[0]["SavePoint"] + "");

        SpawnPlayer();


        //총기 추가 할떄마다 추가하기
    }

    void SpawnPlayer()
    {
        player.transform.position = spawnPoints[spawnPoint].position;
    }

    public void PlayerDead()
    {
        player.enabled = false;
        isPlayerDead = true;
        UIManager.Instance.endPanel.SetActive(true);
        LightManager.Instance.SetDeadVolume();
        cameraArm.enabled = false;
    }

    void Update()
    {

    }

    public void SaveCSVFile(int savePointIndex)
    {
        tempData = new string[8];
        tempData[0] = savePointIndex.ToString();
        tempData[1] = ProgressManager.Instance.curData.ToString();
        tempData[2] = ProgressManager.Instance.saveData.ToString();
        tempData[3] = MaterialManager.Instance.HandgunAmmo.ToString();
        tempData[4] = MaterialManager.Instance.RifleAmmo.ToString();
        tempData[5] = MaterialManager.Instance.ShotgunAmmo.ToString();
        tempData[6] = MaterialManager.Instance.LazerAmmo.ToString();
        tempData[7] = MaterialManager.Instance.UFSData.ToString();
        data.Add(tempData);

        string[][] output = new string[data.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = data[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            sb.AppendLine(string.Join(delimiter, output[i]));
        }

        string filepath = SystemPath.GetPath();

        if (!Directory.Exists(filepath))
        {
            Directory.CreateDirectory(filepath);
        }

        StreamWriter outStream = System.IO.File.CreateText(filepath + wfileName);
        outStream.Write(sb);
        outStream.Close();
    }
}

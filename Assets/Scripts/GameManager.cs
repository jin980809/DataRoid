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
    public string userName;
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

        tempData = new string[17];
        tempData[0] = "SavePoint";
        tempData[1] = "Data";
        tempData[2] = "MaxData";
        tempData[3] = "HandgunAmmo";
        tempData[4] = "RifleAmmo";
        tempData[5] = "ShotgunAmmo";
        tempData[6] = "LazerAmmo";
        tempData[7] = "UFSData";
        tempData[8] = "LoadedHandgun";
        tempData[9] = "LoadedRiffle";
        tempData[10] = "LoadedShotgun";
        tempData[11] = "LoadedLazer";
        tempData[12] = "QuestText";
        tempData[13] = "TargetIcon";
        tempData[14] = "UserName";
        tempData[15] = "BatteryLevel";
        tempData[16] = "StatLevel";
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

        player.weapons[0].GetComponent<Weapon>().curAmmo = int.Parse(dicList[0]["LoadedHandgun"] + "");
        player.weapons[1].GetComponent<Weapon>().curAmmo = int.Parse(dicList[0]["LoadedRiffle"] + "");
        player.weapons[2].GetComponent<Weapon>().curAmmo = int.Parse(dicList[0]["LoadedShotgun"] + "");
        player.weapons[3].GetComponent<Weapon>().curAmmo = int.Parse(dicList[0]["LoadedLazer"] + "");
        UIManager.Instance.questText.text = dicList[0]["QuestText"] + "";
        ObjectManager.Instance.nameTagIndex = int.Parse(dicList[0]["TargetIcon"] + "");
        userName = dicList[0]["UserName"] + "";
        ProgressManager.Instance.batteryLevel = int.Parse(dicList[0]["BatteryLevel"] + "");
        ProgressManager.Instance.statLevel = int.Parse(dicList[0]["StatLevel"] + "");

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
        tempData = new string[17];
        tempData[0] = savePointIndex.ToString();
        tempData[1] = ProgressManager.Instance.curData.ToString();
        tempData[2] = ProgressManager.Instance.saveData.ToString();
        tempData[3] = MaterialManager.Instance.HandgunAmmo.ToString();
        tempData[4] = MaterialManager.Instance.RifleAmmo.ToString();
        tempData[5] = MaterialManager.Instance.ShotgunAmmo.ToString();
        tempData[6] = MaterialManager.Instance.LazerAmmo.ToString();
        tempData[7] = MaterialManager.Instance.UFSData.ToString();
        tempData[8] = player.weapons[0].GetComponent<Weapon>().curAmmo.ToString();
        tempData[9] = player.weapons[1].GetComponent<Weapon>().curAmmo.ToString();
        tempData[10] = player.weapons[2].GetComponent<Weapon>().curAmmo.ToString();
        tempData[11] = player.weapons[3].GetComponent<Weapon>().curAmmo.ToString();
        tempData[12] = UIManager.Instance.questText.text;
        tempData[13] = ObjectManager.Instance.nameTagIndex.ToString();
        tempData[14] = userName;
        tempData[15] = ProgressManager.Instance.batteryLevel.ToString();
        tempData[16] = ProgressManager.Instance.statLevel.ToString();

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

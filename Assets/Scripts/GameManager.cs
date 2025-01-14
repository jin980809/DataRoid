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
    [Header("Player Initialization")]
    public GameObject handGunObj;
    public GameObject riffleObj;
    public GameObject shotGunObj;

    [Space(10)]
    [Header("ESN08")]
    public int ESN08Phase2Diff = 0;

    List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();

    void Awake()
    {
        data.Clear();

        tempData = new string[8];
        tempData[0] = "SavePoint";
        tempData[1] = "Progress";
        tempData[2] = "MaxProgress";
        tempData[3] = "Ammo";
        tempData[4] = "SpecialAmmo";
        tempData[5] = "Steel";
        tempData[6] = "GunPowder";
        tempData[7] = "UFSData";
        data.Add(tempData);
    }

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
        MaterialManager.Instance.UFSData = int.Parse(dicList[0]["UFSData"] + "");

        spawnPoint = int.Parse(dicList[0]["SavePoint"] + "");

        SpawnPlayer();

        if(handGunObj.activeSelf == false)
        {
            player.hasWeapons[1] = true;
        }

        //�ѱ� �߰� �ҋ����� �߰��ϱ�
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

    public void SaveCSVFile(int savePointIndex)
    {
        tempData = new string[8];
        tempData[0] = savePointIndex.ToString();
        tempData[1] = ProgressManager.Instance.curProgress.ToString();
        tempData[2] = ProgressManager.Instance.saveProgress.ToString();
        tempData[3] = MaterialManager.Instance.Ammo.ToString();
        tempData[4] = MaterialManager.Instance.SpecialAmmo.ToString();
        tempData[5] = MaterialManager.Instance.Steel.ToString();
        tempData[6] = MaterialManager.Instance.GunPowder.ToString();
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

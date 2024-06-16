using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<ProgressManager>();
            }

            return m_instance;
        }
    }
    private static ProgressManager m_instance;


    public float curData;
    public float maxData;
    public float saveData;

    //[Space(10)]
    //[Header("Alarm Data")]
    //public int curAlarmData;
    //public int maxAlarmData;
    //public int saveAlarmData;

    //[Space(10)]
    //[Header("Camera Data")]
    //public int curCameraData;
    //public int maxCameraData;
    //public int saveCameraData;

    //[Space(10)]
    //[Header("Slient Data")]
    //public int curSilentData;
    //public int maxSilentData;
    //public int saveSilentData;

    //[Space(10)]
    //[Header("Network Data")]
    //public int curNetworkData;
    //public int maxNetworkData;
    //public int saveNetworkData;

    public Player player;
    //public float dataAverage = 0;
    public int dataLevel;

    public int batteryLevel;
    public int statLevel;

    public string[] DataNames;
    //public string fileName = "Data";
    //List<string[]> data = new List<string[]>();
    //string[] tempData;
    //public string wfileName = "Data.csv";
    //List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();

    [Serializable]
    public struct LevelStat
    {
        public float maxHp;
        public float walkSpeed;
        public float runSpeed;
        public float crouchSpeed;
        public float gunOnWalkSpeed;
        public float gunOnRunSpeed;
        public float fog;
    }

    public LevelStat[] levelPerStat;
    void Awake()
    {
        //data.Clear();

        //tempData = new string[8];
        //tempData[0] = "AlarmData";
        //tempData[1] = "MaxAlarmData";
        //tempData[2] = "CameraData";
        //tempData[3] = "MaxCameraData";
        //tempData[4] = "SilentData";
        //tempData[5] = "MaxSilentData";
        //tempData[6] = "NetworkData";
        //tempData[7] = "MaxNetworkData";
        //data.Add(tempData);

        dataLevel = batteryLevel + statLevel;

        DataLevelStat(statLevel);
        DataLevelBattery(batteryLevel);
    }

    void Start()
    {
        //dicList.Clear();

        //dicList = CSVReader.Read(fileName);

        //curAlarmData = int.Parse(dicList[0]["AlarmData"] + "");
        //maxAlarmData = int.Parse(dicList[0]["MaxAlarmData"] + "");
        //curCameraData = int.Parse(dicList[0]["CameraData"] + "");
        //maxCameraData = int.Parse(dicList[0]["MaxCameraData"] + "");
        //curSilentData = int.Parse(dicList[0]["SilentData"] + "");
        //maxSilentData = int.Parse(dicList[0]["MaxSilentData"] + "");
        //curNetworkData = int.Parse(dicList[0]["NetworkData"] + "");
        //maxNetworkData = int.Parse(dicList[0]["MaxNetworkData"] + "");
    }

    void Update()
    {
        DataLevelUp();

        SetDataName();

        if(dataLevel >= 1)
        {
            player.qSkillOn = true;
        }
    }

    void SetDataName()
    {
        UIManager.Instance.DataName.text = DataNames[dataLevel];
    }

    public void DataLevelStat(int level)
    {  
        player.walkSpeed = levelPerStat[level].walkSpeed;
        player.runSpeed = levelPerStat[level].runSpeed;
        player.crouchSpeed = levelPerStat[level].crouchSpeed;
        player.gunWalkSpeed = levelPerStat[level].gunOnWalkSpeed;
        player.gunRunSpeed = levelPerStat[level].gunOnRunSpeed;
    }

    public void DataLevelBattery(int level)
    {
        player.maxHp = levelPerStat[level].maxHp;
    }

    void DataLevelUp()
    {
        if(curData >= 100)
        { 
            dataLevel += 1;
            UIManager.Instance.LevelPointUIAnim.SetTrigger("Open");
            Time.timeScale = 0f;
            player.isCommunicate = true;
            curData -= 100;
        }
    }

    //public void SaveCSVFile()
    //{
    //    tempData = new string[8];
    //    tempData[0] = curAlarmData.ToString();
    //    tempData[1] = maxAlarmData.ToString();
    //    tempData[2] = curCameraData.ToString();
    //    tempData[3] = maxCameraData.ToString();
    //    tempData[4] = curSilentData.ToString();
    //    tempData[5] = maxSilentData.ToString();
    //    tempData[6] = curNetworkData.ToString();
    //    tempData[7] = maxNetworkData.ToString();
    //    data.Add(tempData);

    //    string[][] output = new string[data.Count][];

    //    for (int i = 0; i < output.Length; i++)
    //    {
    //        output[i] = data[i];
    //    }

    //    int length = output.GetLength(0);
    //    string delimiter = ",";

    //    StringBuilder sb = new StringBuilder();

    //    for (int i = 0; i < length; i++)
    //    {
    //        sb.AppendLine(string.Join(delimiter, output[i]));
    //    }

    //    string filepath = SystemPath.GetPath();

    //    if (!Directory.Exists(filepath))
    //    {
    //        Directory.CreateDirectory(filepath);
    //    }

    //    StreamWriter outStream = System.IO.File.CreateText(filepath + wfileName);
    //    outStream.Write(sb);
    //    outStream.Close();
    //}
}

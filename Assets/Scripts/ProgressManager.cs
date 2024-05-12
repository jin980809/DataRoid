using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

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

    [Space(10)]
    [Header("Alarm Data")]
    public int curAlarmData;
    public int maxAlarmData;
    public int saveAlarmData;

    [Space(10)]
    [Header("Camera Data")]
    public int curCameraData;
    public int maxCameraData;
    public int saveCameraData;

    [Space(10)]
    [Header("Slient Data")]
    public int curSilentData;
    public int maxSilentData;
    public int saveSilentData;

    [Space(10)]
    [Header("Network Data")]
    public int curNetworkData;
    public int maxNetworkData;
    public int saveNetworkData;

    public Player player;
    public float dataAverage = 0;
    public int dataLevel;
    public string fileName = "Data";
    List<string[]> data = new List<string[]>();
    string[] tempData;
    public string wfileName = "Data.csv";
    List<Dictionary<string, object>> dicList = new List<Dictionary<string, object>>();

    void Awake()
    {
        data.Clear();

        tempData = new string[8];
        tempData[0] = "AlarmData";
        tempData[1] = "MaxAlarmData";
        tempData[2] = "CameraData";
        tempData[3] = "MaxCameraData";
        tempData[4] = "SilentData";
        tempData[5] = "MaxSilentData";
        tempData[6] = "NetworkData";
        tempData[7] = "MaxNetworkData";
        data.Add(tempData);
    }

    void Start()
    {
        dicList.Clear();

        dicList = CSVReader.Read(fileName);

        curAlarmData = int.Parse(dicList[0]["AlarmData"] + "");
        maxAlarmData = int.Parse(dicList[0]["MaxAlarmData"] + "");
        curCameraData = int.Parse(dicList[0]["CameraData"] + "");
        maxCameraData = int.Parse(dicList[0]["MaxCameraData"] + "");
        curSilentData = int.Parse(dicList[0]["SilentData"] + "");
        maxSilentData = int.Parse(dicList[0]["MaxSilentData"] + "");
        curNetworkData = int.Parse(dicList[0]["NetworkData"] + "");
        maxNetworkData = int.Parse(dicList[0]["MaxNetworkData"] + "");
    }

    void Update()
    {
        DataAverage();
    }

    void DataAverage()
    {
        dataAverage = (curAlarmData + curCameraData + curNetworkData + curSilentData) / 4f;

        if(dataAverage <= 4f)
        {
            dataLevel = 0;
        }
        else if(dataAverage > 4 && dataAverage <= 34f)
        {
            dataLevel = 1;
        }
        else if(dataAverage > 34 && dataAverage <= 69)
        {
            dataLevel = 2;
        }
        else
        {
            dataLevel = 3;
        }
    }

    public void SaveCSVFile()
    {
        tempData = new string[8];
        tempData[0] = curAlarmData.ToString();
        tempData[1] = maxAlarmData.ToString();
        tempData[2] = curCameraData.ToString();
        tempData[3] = maxCameraData.ToString();
        tempData[4] = curSilentData.ToString();
        tempData[5] = maxSilentData.ToString();
        tempData[6] = curNetworkData.ToString();
        tempData[7] = maxNetworkData.ToString();
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

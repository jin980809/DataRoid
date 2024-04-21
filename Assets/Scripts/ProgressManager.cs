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

    public int curAlarmData;
    public int maxAlarmData;
    public int saveAlarmData;

    public int curCameraData;
    public int maxCameraData;
    public int saveCameraData;

    public int curSilentData;
    public int maxSilentData;
    public int saveSilentData;

    public int curNetworkData;
    public int maxNetworkData;
    public int saveNetworkData;

    public Player player;

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
        MaxProgress();
        UpdateMaxProgress();
        DataGauge();
        ProgressUnlock();
    }
    void UpdateMaxProgress()
    {
        if(curData > saveData)
        {
            saveData = curData;
        }

        if (curAlarmData > saveAlarmData)
        {
            saveAlarmData = curAlarmData;
        }
        if (curCameraData > saveCameraData)
        {
            saveCameraData = curCameraData;
        }
        if (curSilentData > saveSilentData)
        {
            saveSilentData = curSilentData;
        }
        if (curNetworkData > saveNetworkData)
        {
            saveNetworkData = curNetworkData;
        }
    }

    void MaxProgress()
    {
        if (maxData < curData)
        {
            curData = maxData;
        }

        if (maxAlarmData < curAlarmData)
        {
            curAlarmData = maxAlarmData;
        }
        if (maxCameraData < curCameraData)
        {
            curCameraData = maxCameraData;
        }
        if (maxSilentData < curSilentData)
        {
            curSilentData = maxSilentData;
        }
        if (maxNetworkData < curNetworkData)
        {
            curNetworkData = maxNetworkData;
        }
    }

    void DataGauge()
    {
        UIManager.Instance.DataGauge.fillAmount = ((curData / 100f) * 0.45f) + 0.18f;
        UIManager.Instance.Datatext.text = (int)curData + "%";
    }

    void ProgressUnlock()
    {
        //player.qSkillOn = curProgress >= 15 ? true : false;

        if (curData >= 10f)
        {
            player.sheildDamage = 3.7f;
        }

        if (curData >= 10f)
        {
            player.qSkillOn = true;
        }

        if (curData >= 33f)
        {
            player.sheildDamage = 4f;
        }

        if (curData >= 66f)
        {
            player.sheildDamage = 5.5f;
        }

        if (curData >= 100f)
        {
            player.sheildDamage = 6f;
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

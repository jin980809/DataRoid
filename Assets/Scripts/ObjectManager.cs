using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<ObjectManager>();
            }

            return m_instance;
        }
    }
    private static ObjectManager m_instance;

    [Space(10)]
    [Header("Save Object")]
    public bool[] saveObjects;
    public List<Dictionary<string, object>> objectInfo = new List<Dictionary<string, object>>();
    List<string[]> data = new List<string[]>();
    string[] tempData;
    public string wfileName = "Object.csv";
    public string fileName = "Object";

    void Awake()
    {
        LoadCSVFile();

        objectInfo.Clear();

        objectInfo = CSVReader.Read(fileName);

        Initialization();
    }

    void Start()
    {

    }

    void Update()
    {
        //ESN08Clear();
    }

    void Initialization()
    {
        for (int i = 0; i < saveObjects.Length; i++)
        {
            if (int.Parse(objectInfo[i]["Activate"] + "") == 0)
            {
                saveObjects[i] = false;
            }
            else
            {
                saveObjects[i] = true;
            }
        }
    }

    //void ESN08Clear()
    //{
    //    if (ESN08.isDeath)
    //    {
    //        p1ServerRacks.SetActive(false);
    //        p2ServerRacks.SetActive(true);
    //    }
    //    else
    //    {
    //        p1ServerRacks.SetActive(true);
    //        p2ServerRacks.SetActive(false);
    //    }
    //}

    void LoadCSVFile()
    {
        data.Clear(); // 기존 데이터 초기화

        string filepath = SystemPath.GetPath() + wfileName;

        if (File.Exists(filepath))
        {
            string[] lines = File.ReadAllLines(filepath);

            for (int i = 0; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(',');
                data.Add(values);
            }
        }
        else
        {
            Debug.LogWarning("CSV file not found: " + filepath);
        }
    }

    public void SaveObjectFile()
    {
        for (int i = 0; i < saveObjects.Length; i++)
        {
            UpdateCSVFile(i + 1);
        }
    }

    public void UpdateCSVFile(int rowIndex)
    {
        if (rowIndex >= 0 && rowIndex < data.Count)
        {
            tempData = new string[2];
            tempData[0] =  (rowIndex - 1).ToString();
            if(saveObjects[rowIndex - 1])
                tempData[1] = "1";
            else
                tempData[1] = "0";


            data[rowIndex] = tempData; // 특정 줄 업데이트

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
        else
        {
            Debug.LogWarning("Invalid row index for updating CSV file.");
        }
    }
}

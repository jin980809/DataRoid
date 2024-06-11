using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<EnemyManager>();
            }

            return m_instance;
        }
    }
    private static EnemyManager m_instance;

    //[Serializable]
    //public struct EnemyStat
    //{
    //    public int ID;
    //    public float maxHp;
    //    public float maxSheild;
    //    public float Damage;
    //    public bool isDead;
    //}
    //public EnemyStat[] enemyStat;

    public List<Dictionary<string, object>> enemyInfo = new List<Dictionary<string, object>>();

    public string fileName = "Enemy";

    List<string[]> data = new List<string[]>();
    string[] tempData;
    public string wfileName = "Enemy.csv";
    public int enemyInfoLength;

    void Awake()
    {
        LoadCSVFile();

        enemyInfo.Clear();

        if (GameManager.Instance.isNewGame == 0)
        {
            enemyInfo = CSVReader.Read(fileName);
        }
        else
        {
            enemyInfo = CSVReader.Read("Start" + fileName);
        }
    }

    void Start()
    {
        enemyInfoLength = enemyInfo.Count;
    }


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

    void UpdateCSVFile(int rowIndex)
    {
        if (rowIndex >= 0 && rowIndex < data.Count)
        {
            tempData = new string[5];
            tempData[0] = (rowIndex - 1).ToString();
            tempData[1] = enemyInfo[rowIndex - 1]["HP"].ToString();
            tempData[2] = enemyInfo[rowIndex - 1]["Sheild"].ToString();
            tempData[3] = enemyInfo[rowIndex - 1]["Damage"].ToString();
            tempData[4] = enemyInfo[rowIndex - 1]["isDead"].ToString();


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

    public void SaveEnemyData()
    {
        for(int i = 0; i < enemyInfoLength; i++)
        {
            UpdateCSVFile(i + 1);
        }
    }
}

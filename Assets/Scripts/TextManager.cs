using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class TextManager : MonoBehaviour
{
    public static TextManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<TextManager>();
            }

            return m_instance;
        }
    }
    private static TextManager m_instance;

    public bool[] TextObjects;
    public List<Dictionary<string, object>> textInfo = new List<Dictionary<string, object>>();
    List<string[]> data = new List<string[]>();
    string[] tempData;
    public string wfileName = "TextBox.csv";
    public string fileName = "TextBox";

    void Awake()
    {
        LoadCSVFile();

        textInfo.Clear();

        textInfo = CSVReader.Read(fileName);

        Initialization();
    }

    void Initialization()
    {
        for (int i = 0; i < TextObjects.Length; i++)
        {
            if (int.Parse(textInfo[i]["Activate"] + "") == 0)
            {
                TextObjects[i] = false;
            }
            else
            {
                TextObjects[i] = true;
            }
        }
    }

    void LoadCSVFile()
    {
        data.Clear(); // ���� ������ �ʱ�ȭ

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

    public void SaveTextObjectFile()
    {
        for (int i = 0; i < TextObjects.Length; i++)
        {
            UpdateCSVFile(i + 1);
        }
    }

    public void UpdateCSVFile(int rowIndex)
    {
        if (rowIndex >= 0 && rowIndex < data.Count)
        {
            tempData = new string[2];
            tempData[0] = (rowIndex - 1).ToString();
            if (TextObjects[rowIndex - 1])
                tempData[1] = "1";
            else
                tempData[1] = "0";


            data[rowIndex] = tempData; // Ư�� �� ������Ʈ

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
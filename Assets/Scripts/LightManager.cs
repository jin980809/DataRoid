using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    public static LightManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<LightManager>();
            }

            return m_instance;
        }
    }
    private static LightManager m_instance;

    public bool[] lightObjects;
    public List<Dictionary<string, object>> lightInfo = new List<Dictionary<string, object>>();
    List<string[]> data = new List<string[]>();
    string[] tempData;
    public string wfileName = "Light.csv";
    public string fileName = "Light";

    public Volume[] volumes;

    [Space(10)]
    [Header("ID : 0, Tut Light")]
    public GameObject tutLightOn;
    public GameObject tutLightOff;
    public Interaction tutLightInteraction;
    public Collider tutInteractionCol;

    void Awake()
    {
        LoadCSVFile();

        lightInfo.Clear();

        if (GameManager.Instance.isNewGame == 0)
        {
            lightInfo = CSVReader.Read(fileName);
        }
        else
        {
            lightInfo = CSVReader.Read("Start" + fileName);
        }
        
        Initialization();
    }

    void Initialization()
    {
        for (int i = 0; i < lightObjects.Length; i++)
        {
            if (int.Parse(lightInfo[i]["Activate"] + "") == 0)
            {
                lightObjects[i] = false;
            }
            else
            {
                lightObjects[i] = true;
            }
        }
    }

    void Start()
    {
        if(lightObjects[0])
        {
            tutLightInteraction.ObjectOnOff();

            tutLightOn.SetActive(true);
            tutLightOff.SetActive(false);
            tutInteractionCol.enabled = false;
        }
        else
        {
            tutLightOn.SetActive(false);
            tutLightOff.SetActive(true);
        }
    }

    public void SetDeadVolume()
    {
        ColorAdjustments cA;
        for(int i = 0; i < volumes.Length; i++)
        {
            if(volumes[i].profile.TryGet<ColorAdjustments>(out cA))
            {
                StartCoroutine(SetSaturation(cA, 4f));
            }
        }
    }

    IEnumerator SetSaturation(ColorAdjustments cA, float time)
    {
        float etime = 0;

        while(etime < time)
        {
            etime += Time.deltaTime;

            cA.saturation.value = Mathf.Lerp(0, -100, etime/time);

            yield return null;
        }

        yield return null;
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

    public void SaveLightObjectFile()
    {
        for (int i = 0; i < lightObjects.Length; i++)
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
            if (lightObjects[rowIndex - 1])
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

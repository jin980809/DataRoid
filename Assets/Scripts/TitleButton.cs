using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;

public class TitleButton : MonoBehaviour
{
    public int initSpawn;
    public int initProgress;
    public int initMaxProgress;
    public int initHandgunAmmo;
    public int initRifleAmmo;
    public int initShotgunAmmo;
    public int initGunPowder;
    public int initUSFData;

    public List<Dictionary<string, object>> enemyInfo = new List<Dictionary<string, object>>();
    public List<Dictionary<string, object>> LightInfo = new List<Dictionary<string, object>>();
    public List<Dictionary<string, object>> objectInfo = new List<Dictionary<string, object>>();
    public List<Dictionary<string, object>> saveInfo = new List<Dictionary<string, object>>();
    public List<Dictionary<string, object>> textBoxInfo = new List<Dictionary<string, object>>();

    List<string[]> enemydata = new List<string[]>();
    List<string[]> lightdata = new List<string[]>();
    List<string[]> objectdata = new List<string[]>();
    List<string[]> savedata = new List<string[]>();
    List<string[]> textboxdata = new List<string[]>();

    public string enemyfileName = "Enemy.csv";
    public string lightfileName = "Light.csv";
    public string objectfileName = "Object.csv";
    public string savefileName = "Save.csv";
    public string textBoxfileName = "TextBox.csv";

    string[] tempData;
    public string nextSceneName;

    void Awake()
    {
        LoadCSVFile(enemydata, enemyfileName);
        LoadCSVFile(lightdata, lightfileName);
        LoadCSVFile(objectdata, objectfileName);
        LoadCSVFile(savedata, savefileName);
        LoadCSVFile(textboxdata, textBoxfileName);

        enemyInfo.Clear();
        LightInfo.Clear();
        objectInfo.Clear();
        saveInfo.Clear();
        textBoxInfo.Clear();

        enemyInfo = CSVReader.Read("Enemy");
        LightInfo = CSVReader.Read("Light");
        objectInfo = CSVReader.Read("Object");
        saveInfo = CSVReader.Read("Save");
        textBoxInfo = CSVReader.Read("TextBox");
    }


    void LoadCSVFile(List<string[]> data, string wfileName)
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

    public void UpdateCSVFile(int rowIndex, List<string[]> data, string wfileName)
    {
        if (rowIndex >= 0 && rowIndex < data.Count)
        {
            if(wfileName == "Save.csv")
            {
                tempData = new string[8];
                tempData[0] = initSpawn.ToString();
                tempData[1] = initProgress.ToString();
                tempData[2] = initMaxProgress.ToString();
                tempData[3] = initHandgunAmmo.ToString();
                tempData[4] = initRifleAmmo.ToString();
                tempData[5] = initShotgunAmmo.ToString();
                tempData[6] = initGunPowder.ToString();
                tempData[7] = initUSFData.ToString();
            }
            else if(wfileName == "Light.csv")
            {
                tempData = new string[2];
                tempData[0] = (rowIndex - 1).ToString();
                tempData[1] = "0";
            }
            else if(wfileName == "Enemy.csv")
            {
                tempData = new string[5];
                tempData[0] = (rowIndex - 1).ToString();
                tempData[1] = enemyInfo[rowIndex - 1]["HP"].ToString();
                tempData[2] = enemyInfo[rowIndex - 1]["Sheild"].ToString();
                tempData[3] = enemyInfo[rowIndex - 1]["Damage"].ToString();
                tempData[4] = "0";
            }
            else
            {
                tempData = new string[2];
                tempData[0] = (rowIndex - 1).ToString();
                tempData[1] = "1";
            }



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


    public void StartClick()
    {
        //UpdateCSVFile(1, savedata, savefileName);

        //int a = textBoxInfo.Count;

        //for (int i = 0; i < a; i++)
        //{
        //    UpdateCSVFile(i + 1, textboxdata, textBoxfileName);
        //}

        //a = enemyInfo.Count;
        //for (int i = 0; i < a; i++)
        //{
        //    UpdateCSVFile(i + 1, enemydata, enemyfileName);
        //}

        //a = LightInfo.Count;
        //for (int i = 0; i < a; i++)
        //{
        //    UpdateCSVFile(i + 1, lightdata, lightfileName);
        //}

        //a = objectInfo.Count;
        //for (int i = 0; i < a; i++)
        //{
        //    UpdateCSVFile(i + 1, objectdata, objectfileName);
        //}

        SceneManager.LoadScene(nextSceneName);
    }

    public void LoadClick()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void ExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

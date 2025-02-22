using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{


    public string fileName = "Save.csv";
    public Player player;
    List<string[]> data = new List<string[]>();
    string[] tempData;

    void Awake()
    {
        data.Clear();

        tempData = new string[7];
        tempData[0] = "SavePoint";
        tempData[1] = "Progress";
        tempData[2] = "MaxProgress";
        tempData[3] = "Ammo";
        tempData[4] = "SpecialAmmo";
        tempData[5] = "Steel";
        tempData[6] = "GunPowder";
        data.Add(tempData);
    }

    void Start()
    {
        //SaveCSVFile();
    }

    public void SaveCSVFile(int savePointIndex)
    {
        tempData = new string[7];
        tempData[0] = savePointIndex.ToString();
        tempData[1] = ProgressManager.Instance.curProgress.ToString();
        tempData[2] = ProgressManager.Instance.saveProgress.ToString();
        tempData[3] = MaterialManager.Instance.Ammo.ToString();
        tempData[4] = MaterialManager.Instance.SpecialAmmo.ToString();
        tempData[5] = MaterialManager.Instance.Steel.ToString();
        tempData[6] = MaterialManager.Instance.GunPowder.ToString();
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

        StreamWriter outStream = System.IO.File.CreateText(filepath + fileName);
        outStream.Write(sb);
        outStream.Close();
    }
}

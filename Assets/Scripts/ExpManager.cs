using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpManager : MonoBehaviour
{
    public static ExpManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<ExpManager>();
            }

            return m_instance;
        }
    }
    private static ExpManager m_instance;

    [SerializeField] private int curLevel = 1;
    public int curExp = 0;

    [SerializeField] private int requireExp;
    List<Dictionary<string, object>> expData;

    void Start()
    {
        expData = CSVReader.Read("Exp");
        requireExp = int.Parse(expData[curLevel - 1]["Require Exp"].ToString());
    }

    void Update()
    {
        LevelUp();
        ExpGauge();
    }

    void LevelUp()
    {
        if (curExp > requireExp)
        {
            curExp = curExp - requireExp;
            requireExp = int.Parse(expData[++curLevel - 1]["Require Exp"].ToString());
        }
    }

    void ExpGauge()
    {
        /*UIManager.Instance.ExpGauge.value = (curExp + 0.001f) / requireExp;*/
    }
}

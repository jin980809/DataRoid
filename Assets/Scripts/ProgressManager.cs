using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    public float curProgress;
    public float maxProgress;
    public float saveProgress;

    public Player player;

    void Start()
    {
        
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
        if(curProgress > saveProgress)
        {
            saveProgress = curProgress;
        }
    }

    void MaxProgress()
    {
        if(maxProgress < curProgress)
        {
            curProgress = maxProgress;
        }
    }

    void DataGauge()
    {
        UIManager.Instance.ExpGauge.value = (curProgress + 0.001f) / maxProgress;
    }

    void ProgressUnlock()
    {
        //player.qSkillOn = curProgress >= 15 ? true : false;

        if (curProgress >= 10f)
        {
            player.sheildDamage = 3.7f;
        }

        if (curProgress >= 15f)
        {
            player.qSkillOn = true;
        }

        if (curProgress >= 33f)
        {
            player.sheildDamage = 4f;
        }

        if (curProgress >= 66f)
        {
            player.sheildDamage = 5.5f;
        }

        if (curProgress >= 100f)
        {
            player.sheildDamage = 6f;
        }
    }

}

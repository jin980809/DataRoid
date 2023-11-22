using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CreateManager : MonoBehaviour
{
    public static CreateManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<CreateManager>();
            }

            return m_instance;
        }
    }
    private static CreateManager m_instance;

    public enum Type
    {
        Ammo = 0,
        SpecialAmmo = 1 
    };

    [Serializable]
    public struct Creating
    {
        public int steel;
        public int gunPowder;
        public float creatingTime;
        public Type ResultType;
        public int resultAmount;
    }

    public Creating[] creats;
    public bool isCreating = false;
    public float curCreatingTime;

    void Update()
    {
        DecreaseTime();
    }

    void DecreaseTime()
    {
        if (isCreating)
        {
            curCreatingTime -= Time.deltaTime;

            if (curCreatingTime <= 0)
            {
                isCreating = false;
            }

            //Debug.Log(curCreatingTime);
        }
    }
}

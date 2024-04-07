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
        SpecialAmmo = 1,
        ExpCapsule = 2
    };

    [Serializable]
    public struct Creating
    {
        public int steel;
        public int gunPowder;
        public int expPiece;
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

    IEnumerator CreateResult(int ID)
    {
        yield return new WaitForSeconds(creats[ID].creatingTime);

        /*MaterialManager.Instance.Steel -= creats[ID].steel;
        MaterialManager.Instance.GunPowder -= creats[ID].gunPowder;
        MaterialManager.Instance.ExpPiece -= creats[ID].expPiece;

        switch (creats[ID].ResultType)
        {
            case Type.Ammo:
                MaterialManager.Instance.Ammo += creats[ID].resultAmount;
                break;

            case Type.SpecialAmmo:
                MaterialManager.Instance.SpecialAmmo += creats[ID].resultAmount;
                break;

            case Type.ExpCapsule:
                MaterialManager.Instance.ExpCapsule += creats[ID].resultAmount;
                break;
        }

        UIManager.Instance.CreatingText.SetActive(false);*/
    }

    public void CreatingStart(int ID)
    {
        StartCoroutine(CreateResult(ID));
    }    
}

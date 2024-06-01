using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    public static MaterialManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<MaterialManager>();
            }

            return m_instance;
        }
    }
    private static MaterialManager m_instance;

    public Player player;

    public int ShotgunAmmo;
    public int LazerAmmo;
    public int ExpPiece;
    public int HandgunAmmo;
    public int RifleAmmo;
    public int ExpCapsule;
    public int UFSData;

}

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



    public int Steel;
    public int GunPowder;
    public int ExpPiece;
    public int Ammo;
    public int SpecialAmmo;
    public int ExpCapsule;
}

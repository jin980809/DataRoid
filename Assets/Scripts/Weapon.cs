using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Weapon : MonoBehaviour
{
    public int damage;
    public float delay;
    public int maxAmmo;
    public int curAmmo;
    public float reLoadTime;
    public bool isSemiauto;
    public GameObject muzzleFlash;
    public bool isShotGun;
    public int shotGunSpreadAmount;
    public float shotGunRange;
    public float inaccuracyDistance;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

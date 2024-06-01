using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Weapon : MonoBehaviour
{
    public int damage;
    public float bulletSpeed;
    public float delay;
    public int maxAmmo;
    public int curAmmo;
    public float reLoadTime;
    public float reloadCost;
    public bool isSemiauto;
    public GameObject muzzleFlash;
    public bool isShotGun;
    public int shotGunSpreadAmount;
    public float shotGunRange;
    public float inaccuracyDistance;
    public bool isLazer;
    public Hovl_DemoLasers lazerEffect;

}

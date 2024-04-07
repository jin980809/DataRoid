using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAmmo : MonoBehaviour
{
    public Player player;
    public int weaponIndex;
    public float decreaseHP;
    public int getAmmo;

    public void Onclick()
    {
        player.curHp -= decreaseHP;

        switch(weaponIndex)
        {
            case 0:
                MaterialManager.Instance.RifleAmmo += getAmmo;
                break;

            case 1:
                MaterialManager.Instance.HandgunAmmo += getAmmo;
                break;

            case 2:
                MaterialManager.Instance.ShotgunAmmo += getAmmo;
                break;
        }
    }
}

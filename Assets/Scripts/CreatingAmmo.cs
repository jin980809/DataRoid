using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatingAmmo : MonoBehaviour
{
    public float decreaseHp;
    public int ammoAmount;
    public float creatingTime;

    public void OnClick()
    {
        StartCoroutine(Result());
        //���� ǥ�� �����
    }

    IEnumerator Result()
    {
        yield return new WaitForSeconds(creatingTime);
        GameManager.Instance.player.curHp -= decreaseHp;
        MaterialManager.Instance.Ammo += ammoAmount;
    }
}

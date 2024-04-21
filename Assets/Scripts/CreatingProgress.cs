using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatingProgress : MonoBehaviour
{
    public float decreaseHp;
    public int progressAmount;
    public float creatingTime;

    public void OnClick()
    {
        GameManager.Instance.player.curHp -= decreaseHp;
        ProgressManager.Instance.curData += progressAmount;
    }
}

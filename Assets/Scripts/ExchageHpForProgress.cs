using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExchageHpForProgress : MonoBehaviour
{
    public Player player;
    public float decreaseHP;
    public float increaseProgress;

    void Update()
    {
        Interactable();
    }

    void Interactable()
    {
        if (ProgressManager.Instance.curData + increaseProgress > ProgressManager.Instance.saveData)
        {
            GetComponent<Button>().interactable = false;
        }
        else
        {
            GetComponent<Button>().interactable = true;
        }
    }

    public void OnClick()
    {
        player.curHp -= decreaseHP;
        ProgressManager.Instance.curData += increaseProgress;
    }
}

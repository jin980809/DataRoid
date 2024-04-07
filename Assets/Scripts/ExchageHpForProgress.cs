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
        if (ProgressManager.Instance.curProgress + increaseProgress > ProgressManager.Instance.saveProgress)
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
        ProgressManager.Instance.curProgress += increaseProgress;
    }
}

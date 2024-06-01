using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPointSelect : MonoBehaviour
{
    public Player player;
    public void ExitPanel()
    {
        Time.timeScale = 1f;
        player.isCommunicate = false;
        transform.gameObject.SetActive(false);
    }

    public void SelectStatLevel()
    {
        ProgressManager.Instance.statLevel += 1;
        ExitPanel();
    }

    public void SelectSkillLevel()
    {
        ProgressManager.Instance.batteryLevel += 1;
        ExitPanel();
    }


}

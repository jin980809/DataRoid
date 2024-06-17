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
        StartCoroutine(ClosePanel());
        //transform.gameObject.SetActive(false);
    }

    IEnumerator ClosePanel()
    {
        yield return new WaitForSeconds(1);
        UIManager.Instance.LevelPointUIAnim.SetTrigger("Close");
        UIManager.Instance.upgradeUI.SetActive(false);
    }

    public void SelectStatLevel()
    {
        ProgressManager.Instance.statLevel += 1;
        UIManager.Instance.LevelUpAnim.SetTrigger("LevelUp");
        ExitPanel();
    }

    public void SelectBatteryLevel()
    {
        ProgressManager.Instance.batteryLevel += 1;
        UIManager.Instance.LevelUpAnim.SetTrigger("LevelUp");
        ExitPanel();
    }
}

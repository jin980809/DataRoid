using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackingPanel : MonoBehaviour
{
    bool qDown;
    public GameObject EnemyInfo;

    void Update()
    {
        GetInput();

        UIUpdate();

        UIClose();
    }
    void GetInput()
    {
        qDown = Input.GetButton("Cancel");
    }

    void UIUpdate()
    {
        if (EnemyInfo != null)
        {
            UIManager.Instance.EnemyModelName.text = EnemyInfo.GetComponent<Enemy>().modelName;
            UIManager.Instance.IMEI.text = EnemyInfo.GetComponent<Enemy>().iMEI;
            UIManager.Instance.EnemyElec.text = EnemyInfo.GetComponent<Enemy>().curHealth.ToString("F2");
        }
        else
        {
            UIManager.Instance.EnemyModelName.text = "Missing";
            UIManager.Instance.IMEI.text = "Missing";
            UIManager.Instance.EnemyElec.text = "Missing";
        }
    }

    void UIClose()
    {
        if(qDown)
        {
            this.gameObject.SetActive(false);
        }
    }
}

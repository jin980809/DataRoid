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
            UIManager.Instance.HitArea.text = "Critical Area : " + EnemyEnumToString((int)EnemyInfo.GetComponent<Enemy>().hitAreaType);
        }
        else
        {
            UIManager.Instance.EnemyModelName.text = "Missing";
            UIManager.Instance.IMEI.text = "Missing";
            UIManager.Instance.EnemyElec.text = "Missing";
            UIManager.Instance.HitArea.text = "Missing";
        }
    }

    string EnemyEnumToString(int n)
    {
        switch (n)
        {
            case 0:
                return "Head";
            case 1:
                return "Body";
            case 2:
                return "LeftArm";
            case 3:
                return "RightArm";
            case 4:
                return "LeftLeg";
            case 5:
                return "RightLeg";
        }

        return "Missing";
    }

    void UIClose()
    {
        if(qDown)
        {
            this.gameObject.SetActive(false);
        }
    }
}

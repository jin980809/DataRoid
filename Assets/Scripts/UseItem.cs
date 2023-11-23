using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseItem : MonoBehaviour
{
    public int ExpAmount;

    public enum Type
    {
        ExpCapsule = 1
    };
    public Type MarterialType;

    void Update()
    {
        ButtonEnabled();
    }

    public void OnClick()
    {
        switch (MarterialType)
        {
            case Type.ExpCapsule:
                UseExpCapsule();
                break;
        }
    }

    void UseExpCapsule()
    {
        MaterialManager.Instance.ExpCapsule--;
        ExpManager.Instance.curExp += ExpAmount;
    }

    void ButtonEnabled()
    {
        switch (MarterialType)
        {
            case Type.ExpCapsule:
                if(MaterialManager.Instance.ExpCapsule <= 0)
                {
                    this.GetComponent<Button>().interactable = false;
                }
                else
                {
                    this.GetComponent<Button>().interactable = true;
                }
                break;
        }
    }
}

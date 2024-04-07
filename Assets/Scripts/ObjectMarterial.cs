using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMarterial : MonoBehaviour
{
    public int getMarterialAmount;
    public float dataAmount;
    public enum Type
    {
        Steel = 1,
        GunPowder = 2,
        ExpPiece = 3,
        DropExp = 4,
        Data = 5
    };
    public Type MarterialType;

    void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("Player"))
        {
            switch(MarterialType)
            {
                case Type.Steel:
                    MaterialManager.Instance.Steel += getMarterialAmount;
                    break;

                case Type.GunPowder:
                    MaterialManager.Instance.GunPowder += getMarterialAmount;
                    break;

                case Type.ExpPiece:
                    MaterialManager.Instance.ExpPiece += getMarterialAmount;
                    break;

                case Type.DropExp:
                    ExpManager.Instance.curExp += getMarterialAmount;
                    break;

                case Type.Data:
                    ProgressManager.Instance.curProgress += dataAmount;
                    break;
            }
            
            Destroy(this.gameObject);
        }*/
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateButton : MonoBehaviour
{
    [SerializeField] private int ID;
    CreateManager.Creating create;

    void Start()
    {
        create = CreateManager.Instance.creats[ID];
    }

    void Update()
    {
        ButtonEnabled();
    }

    public void OnClick()
    {

        if (!CreateManager.Instance.isCreating)
        {
            CreateManager.Instance.curCreatingTime = create.creatingTime;
            CreateManager.Instance.isCreating = true;
            //코루틴 시작
            UIManager.Instance.CreatingText.SetActive(true);
            CreateManager.Instance.CreatingStart(ID);
            transform.parent.GetComponent<CreatingUI>().CreatingUIOut();
        }
    }

    void ButtonEnabled()
    {
        /*if (MaterialManager.Instance.Steel >= create.steel && 
            MaterialManager.Instance.GunPowder >= create.gunPowder &&
            MaterialManager.Instance.ExpPiece >= create.expPiece &&
            !CreateManager.Instance.isCreating)
        {
            this.GetComponent<Button>().interactable = true;
        }
        else
        {
            this.GetComponent<Button>().interactable = false;
        }*/
    }

}

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
            StartCoroutine(CreateResult());
        }
    }

    IEnumerator CreateResult()
    {
        yield return new WaitForSeconds(create.creatingTime);

        MaterialManager.Instance.Steel -= create.steel;
        MaterialManager.Instance.GunPowder -= create.gunPowder;

        switch (create.ResultType)
        {
            case CreateManager.Type.Ammo:
                MaterialManager.Instance.Ammo += create.resultAmount;
                break;

            case CreateManager.Type.SpecialAmmo:
                MaterialManager.Instance.SpecialAmmo += create.resultAmount;
                break;
        }

        UIManager.Instance.CreatingText.SetActive(false);
    }

    void ButtonEnabled()
    {
        if (MaterialManager.Instance.Steel >= create.steel && MaterialManager.Instance.GunPowder >= create.gunPowder &&
            !CreateManager.Instance.isCreating)
        {
            this.GetComponent<Button>().interactable = true;
        }
        else
        {
            this.GetComponent<Button>().interactable = false;
        }
    }

}

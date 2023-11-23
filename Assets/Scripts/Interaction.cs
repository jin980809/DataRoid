using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public enum Type
    {
        ElecCharge = 1,
        ConnectCCTV = 2,
        Door = 3,
        Creating = 4
    };
    public Type interactionType;

    public float coolTime;
    private float curCoolTime;
    public float interactionTime;
    public bool isCoolTime = false;
    public Player player;

    [Space(10)]
    [Header("ChargeElec")]
    public float healAmount;

    [Space(10)]
    [Header("ConnectCCTV")]
    public Camera mainCamera;
    public GameObject CCTV;
    [Header("ConnectCCTV / Creating")]
    public CameraMove cameraMove;
    [Header("Creating")]
    public SetCursor setCursor;

    [Space(10)]
    [Header("Door")]
    public float openSpeed;
    public bool isOpen;
    public GameObject leftDoor;
    public GameObject rightDoor;
    public float openTime;


    void Start()
    {
        curCoolTime = coolTime;
    }

    void Update()
    {
        CoolDown();
    }

    void CoolDown()
    {
        if(isCoolTime)
        {
            curCoolTime -= Time.deltaTime;

            if(curCoolTime < 0)
            {
                isCoolTime = false;
            }
        }
        else
        {
            curCoolTime = coolTime;
        }
    }

    public void InteractionResult()
    {
        switch(interactionType)
        {
            case Type.ElecCharge:
                ElecCharge();
                break;

            case Type.ConnectCCTV:
                ConnectCCTV();
                break;

            case Type.Door:
                DoorOpen();
                break;

            case Type.Creating:
                OpenCreatingUI();
                break;

        }
    }

    void ElecCharge()
    {
        player.curHp += healAmount;

        if (player.curHp > player.maxHp)
            player.curHp = player.maxHp;
    }

    void ConnectCCTV()
    {
        mainCamera.enabled = false;
        player.enabled = false;
        cameraMove.enabled = false;

        CCTV.SetActive(true);
    }

    void DoorOpen()
    {
        OpenDoor();
        Invoke("CloseDoor", openTime);
    }

    IEnumerator MoveLeftDoor(GameObject c)
    {
        float elapsedTime = 0f;

        while (elapsedTime < 2f)
        {
            c.transform.localPosition += Vector3.forward * Time.deltaTime * openSpeed;

            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }

    IEnumerator MoveRightDoor(GameObject c)
    {
        float elapsedTime = 0f;

        while (elapsedTime < 2f)
        {
            c.transform.localPosition -= Vector3.forward * Time.deltaTime * openSpeed;

            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }

    void OpenDoor()
    {
        StartCoroutine(MoveLeftDoor(leftDoor));
        StartCoroutine(MoveRightDoor(rightDoor));
        isOpen = true;
    }

    void CloseDoor()
    {
        StartCoroutine(MoveRightDoor(leftDoor));
        StartCoroutine(MoveLeftDoor(rightDoor));
        isOpen = false;
    }

    public void OpenCreatingUI()
    {
        player.enabled = false;
        cameraMove.enabled = false;
        player.isCreaingUIOpen = true;
        UIManager.Instance.CreatingUI.SetActive(true);
    }
}

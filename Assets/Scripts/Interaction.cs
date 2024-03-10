using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    public enum Type
    {
        ElecCharge = 1,
        ConnectCCTV = 2,
        Door = 3,
        SavePoint = 4,
        MovePlayer = 5,
        ObjecctActive = 6,
        GunActive = 7,
        GetUSFData = 8,
        LightOn = 9,
    };
    public Type interactionType;

    public bool isNonCharging;
    public Player player;

    [Space(10)]
    [Header("Save Object")]
    public bool isSaveObject;
    public int ObjectID;

    [Space(10)]
    [Header("Charging Interaction")]
    public float coolTime;
    private float curCoolTime;
    public float interactionTime;
    public bool isCoolTime = false;

    [Space(10)]
    [Header("ChargeElec")]
    public float healAmount;

    [Space(10)]
    [Header("ConnectCCTV")]
    public Camera mainCamera;
    public GameObject CCTV;
    public CameraMove cameraMove;


    [Space(10)]
    [Header("Door")]
    public float openSpeed;
    public bool isOpen;
    public GameObject leftDoor;
    public GameObject rightDoor;
    public float openTime;

    [Space(10)]
    [Header("SavePoint")]
    public int savePoint;


    [Space(10)]
    [Header("MovePlayer")]
    public Transform movePoint;

    [Space(10)]
    [Header("ObjectActive")]
    public bool activeTrue;
    public GameObject activeObj;

    [Space(10)]
    [Header("GetGun")]
    public int gunIndex;

    [Space(10)]
    [Header("GetUFSData")]
    public int DataCount;

    [Space(10)]
    [Header("Light On")]
    public int lightIndex;
    public GameObject offLight;
    public GameObject OnLight;
    public int needData;

    void Awake()
    {
        if (isSaveObject)
        {
            transform.gameObject.SetActive(ObjectManager.Instance.saveObjects[ObjectID]);
        }
    }

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
        if(isCoolTime && !isNonCharging)
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

            case Type.SavePoint:
                GameManager.Instance.SaveCSVFile(savePoint);
                ObjectManager.Instance.SaveObjectFile();
                EnemyManager.Instance.SaveEnemyData();
                TextManager.Instance.SaveTextObjectFile();
                LightManager.Instance.SaveLightObjectFile();
                break;

            case Type.MovePlayer:
                MovePlayer();
                break;

            case Type.ObjecctActive:
                ObjectActive();
                break;

            case Type.GunActive:
                GunActive();
                break;

            case Type.GetUSFData:
                GetData();
                break;

            case Type.LightOn:
                LightOn();
                break;
        }

        if(isSaveObject)
        {
            ObjectManager.Instance.saveObjects[ObjectID] = false;
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

 

    void MovePlayer()
    {
        player.transform.position = movePoint.position;
    }

    void ObjectActive()
    {
        activeObj.SetActive(activeTrue);
        transform.gameObject.SetActive(false);
        ObjectManager.Instance.saveObjects[ObjectID] = false;
    }

    void GunActive()
    {
        player.hasWeapons[gunIndex] = true;
        transform.gameObject.SetActive(false);
        ObjectManager.Instance.saveObjects[ObjectID] = false;
    }

    void GetData()
    {
        MaterialManager.Instance.UFSData += DataCount;
        transform.gameObject.SetActive(false);
        ObjectManager.Instance.saveObjects[ObjectID] = false;
    }

    void LightOn()
    {
        if (MaterialManager.Instance.UFSData > needData)
        {
            offLight.SetActive(false);
            OnLight.SetActive(true);
            GetComponent<BoxCollider>().enabled = false;
            LightManager.Instance.lightObjects[lightIndex] = true;
            MaterialManager.Instance.UFSData -= needData;
        }

    }
}

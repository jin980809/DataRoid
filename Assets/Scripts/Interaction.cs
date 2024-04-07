using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Interaction : FadeController
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
        TextBox = 10,
    };
    public Type interactionType;

    public bool isNonCharging;
    public bool isFade;
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
    public bool isDecreaseHp;
    public float decreaseHp;

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
    public int needUFSData;
    public float getData;

    [Space(10)]
    [Header("TextBox")]

    public TextBox[] textBox;
    public int curTextCount = 0;
    public bool isClick;
    bool isCommunicate = false;
    bool isNextReady = false;
    bool fDown; // АјАн

    [Serializable]
    public struct TextBox
    {
        [TextArea]
        public string droneText;
        public float duration;
    }

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

        fDown = Input.GetButtonDown("Fire1");

        if (fDown && isCommunicate && isClick && isNextReady)
        {
            isNextReady = false;
            StartCoroutine(TextBoxOut(textBox[curTextCount].duration));
        }
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
                player.curHp = player.maxHp;
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

            case Type.TextBox:
                TextOn();
                break;
        }

        if(isSaveObject)
        {
            ObjectManager.Instance.saveObjects[ObjectID] = false;
        }

        if(isFade)
        {
            FadeInOut();
        }

        //if(GetComponent<ObjectNameUI>() != null )
        //{
        //    GetComponent<ObjectNameUI>().nameUI.SetActive(false);
        //}
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
        StartCoroutine(MovePlayer(1.5f));
    }

    IEnumerator MovePlayer(float second)
    {
        yield return new WaitForSeconds(second);
        player.transform.position = movePoint.position;
    }

    void ObjectActive()
    {
        activeObj.SetActive(activeTrue);
        transform.gameObject.SetActive(false);
        ObjectManager.Instance.saveObjects[ObjectID] = false;

        if(isDecreaseHp)
        {
            player.curHp -= decreaseHp;
        }
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
        UIManager.Instance.OpenObjectGetText("Get Data");
    }

    void LightOn()
    {
        if (MaterialManager.Instance.UFSData > needUFSData)
        {
            offLight.SetActive(false);
            OnLight.SetActive(true);
            GetComponent<BoxCollider>().enabled = false;
            LightManager.Instance.lightObjects[lightIndex] = true;
            MaterialManager.Instance.UFSData -= needUFSData;
            ProgressManager.Instance.curProgress += getData;
        }

    }

    void TextOn()
    {
        StartCoroutine(StartTextBox(textBox[curTextCount].duration));
        GetComponent<BoxCollider>().enabled = false;
        if (isClick)
        {
            player.isCommunicate = true;
            isCommunicate = true;
        }
    }

    IEnumerator StartTextBox(float durationTime)
    {
        UIManager.Instance.uiAnim.SetTrigger("Text_Open");
        UIManager.Instance.DroneText.text = textBox[curTextCount].droneText;

        if (!isClick)
        {
            yield return new WaitForSeconds(durationTime);
            StartCoroutine(TextBoxOut(textBox[curTextCount].duration));
        }
        isNextReady = true;
        yield return null;
    }
    IEnumerator TextBoxOut(float durationTime)
    {
        UIManager.Instance.uiAnim.SetTrigger("Text_Out");

        yield return new WaitForSeconds(1f);
        if (curTextCount < textBox.Length - 1)
        {
            curTextCount += 1;

            StartCoroutine(StartTextBox(durationTime));
        }
        else
        {
            if (isClick)
            {
                player.isCommunicate = false;
                isCommunicate = false;
                isNextReady = false;
            }
        }
        yield return null;
    }
}

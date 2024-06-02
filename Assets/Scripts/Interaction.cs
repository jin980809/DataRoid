using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Interaction : FadeController
{
    public enum Type
    {
        ElecCharge = 1,
        ConnectCCTV = 2,
        Door = 3,
        SavePoint = 4,
        MovePlayer = 5,
        ObjectActive = 6,
        GunActive = 7,
        GetUSFData = 8,
        LightOn = 9,
        TextBox = 10,
        PassWord = 11,
        ObjectRotater = 12,
        ObjectOn = 13,
        WeaponBox = 14,
        RotationObject = 15,
    };
    public Type interactionType;

    [Space(10)]
    [Header("Setting")]
    public bool isNonCharging;
    public bool isFade;
    public bool isTextOn;
    public bool isQuestUpdate;
    public bool isNameTagOn;
    public bool isGetData;
    public bool isTrigger;
    public bool dontDestroy;
    public bool weaponDrop;
    public Player player;

    [Space(10)]
    [Header("Quest Text")]
    [SerializeField]
    public string questText;
    public bool useUSFDataVariation;
    public string questTextPlus;

    [Space(10)]
    [Header("NameTag On Off")]
    public int nameTagIndex;

    [Space(10)]
    [Header("GetData")]
    public int getDataAmount;
    public bool hasData;


    [Space(10)]
    [Header("Weapon Drop")]
    public int weaponDropIndex;


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
    public int needLV;
    public float openSpeed;
    public bool isOpen;
    public GameObject leftDoor;
    public GameObject rightDoor;
    public float openTime;
    public bool isNotClose;

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
    bool isText;
    bool fDown;
    public bool textEndObjectOn;
    public bool isOnce;

    [Space(10)]
    [Header("PassWord")]
    public bool isActive = false;
    public Transform cameraPos;
    public GameObject passWordUI;
    private PassWord passWord;
    public GameObject canvas;
    public CameraMove p_cameraMove;
    public GameObject v_Cam;
    public float smoothSpeed;
    public GameObject m_Camera;

    [Space(10)]
    [Header("ObjectRotater")]
    public GameObject p_Object;
    public GameObject data_p_Object;
    private GameObject inst_Object;
    private GameObject data_inst_Object;
    public Transform ObjectPos;
    public GameObject main_Camera;
    public GameObject rotate_Camera;
    public Transform rotate_Camera_Pos;
    private ObjectRotater objRotater;
    public bool hasUFSData;

    [Space(10)]
    [Header("ObjectOn")]
    public int o_needUSFData;

    [Header("Active")]
    public Collider[] a_col;
    public GameObject[] a_gameObj;
    public Interaction[] a_interaction;

    [Header("Disable")]
    public Collider[] d_col;
    public GameObject[] d_gameObj;
    public Interaction[] d_interaction;

    [Serializable]
    public struct TextBox
    {
        [TextArea]
        public string droneText;
        public float duration;
    }

    [Space(10)]
    [Header("WeaponBox")]
    public int weaponIndex;
    public int weaponAmount;
    public bool hasOpen = false;

    [Space(10)]
    [Header("Rotation Object")]
    public GameObject targetObject;
    public float rotationSpeed = 10f;
    public float targetAngle = 90f;
    private float currentAngle = 0f;
    private bool isRotate = false;
    void Awake()
    {

    }

    void Start()
    {
        if (isSaveObject)
        {
            if (interactionType == Type.ObjectRotater)
            {
                hasUFSData = ObjectManager.Instance.saveObjects[ObjectID]; //true 면 데이터 있음 / false면 데이터 없음(이미 습득)
            }
            else if(interactionType == Type.ObjectOn && !ObjectManager.Instance.saveObjects[ObjectID])
            {
                ObjectOnOff();
                transform.gameObject.SetActive(false);
            }
            else if(interactionType == Type.WeaponBox)
            {
                hasOpen = !ObjectManager.Instance.saveObjects[ObjectID];
            }
            else
            {
                if (!dontDestroy)
                    transform.gameObject.SetActive(ObjectManager.Instance.saveObjects[ObjectID]);
            }

            if (isGetData)
            {
                hasData = !ObjectManager.Instance.saveObjects[ObjectID];
            }
        }

        curCoolTime = coolTime;

        if (interactionType == Type.PassWord)
        {
            passWord = passWordUI.GetComponent<PassWord>();
        }


    }

    void Update()
    {
        CoolDown();

        fDown = Input.GetButtonDown("Fire1");

        if (fDown && isCommunicate && isClick && isNextReady && interactionType == Type.TextBox)
        {
            isNextReady = false;
            StartCoroutine(TextBoxOut(textBox[curTextCount].duration));
        }

        if (interactionType == Type.PassWord)
        {
            if (passWord.isDone)
            {
                v_Cam.SetActive(true);
                p_cameraMove.enabled = true;

                transform.gameObject.SetActive(false);
            }

            //if (!passWordUI.activeSelf && !isActive)
            //{
            //    //v_Cam.SetActive(true);
            //    //p_cameraMove.enabled = true;
            //}
        }

        if(interactionType == Type.ObjectRotater)
        {
            if (hasUFSData == false && isSaveObject)
            {
                ObjectManager.Instance.saveObjects[ObjectID] = false;
            }
        }
        //if (interactionType == Type.ObjectRotater)
        //{
        //    if (inst_Object != null)
        //    {
        //        if(!inst_Object.activeSelf)
        //        {
        //            rotate_Camera.SetActive(false);
        //            main_Camera.SetActive(true);
        //        }
        //    }
        //}
    }

    void UIObjectOn()
    {
        if (isQuestUpdate)
        {
            UIManager.Instance.questUIAnim.SetTrigger("QuestUpdate");

            if (useUSFDataVariation)
            {
                UIManager.Instance.questText.text = questText + MaterialManager.Instance.UFSData.ToString() + questTextPlus;
            }
            else
            {
                UIManager.Instance.questText.text = questText;
            }
        }

        if(isNameTagOn)
        {
            ObjectManager.Instance.SetNameTag(nameTagIndex);
        }

        if(isGetData && !hasData)
        {
            ProgressManager.Instance.curData += getDataAmount;
            UIManager.Instance.GetDataUI(getDataAmount);
            hasData = true;
        }

        if(weaponDrop)
        {
            player.weapons[weaponDropIndex].GetComponent<Weapon>().curAmmo += 1;
            player.hasWeapons[weaponDropIndex] = true;
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

    void SaveObject()
    {
        if (interactionType == Type.ObjectRotater)
        {
            if (hasUFSData == false && isSaveObject)
            {
                ObjectManager.Instance.saveObjects[ObjectID] = false;
            }
        }
        else
        {
            if (isSaveObject)
            {
                ObjectManager.Instance.saveObjects[ObjectID] = false;
            }
        }
    }

    public void InteractionResult()
    {
        if (isFade)
        {
            FadeInOut();
        }

        switch (interactionType)
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

            case Type.ObjectActive:
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

            case Type.PassWord:
                PassWordOn();
                break;

            case Type.ObjectRotater:
                ObjectRotaterOpen();
                break;

            case Type.ObjectOn:
                ObjectOn();
                break;

            case Type.WeaponBox:
                OpenWeaponBox();
                break;    
                
            case Type.RotationObject:
                RotateObject();
                break;
        }

        //if(GetComponent<ObjectNameUI>() != null )
        //{
        //    GetComponent<ObjectNameUI>().nameUI.SetActive(false);
        //}
    }

    void ElecCharge()
    {
        SaveObject();
        player.curHp += healAmount;

        if (player.curHp > player.maxHp)
            player.curHp = player.maxHp;

        UIManager.Instance.GetBatteryUI((int)healAmount);

        transform.gameObject.SetActive(false);
    }

    void ConnectCCTV()
    {
        SaveObject();
        mainCamera.enabled = false;
        player.enabled = false;
        cameraMove.enabled = false;

        CCTV.SetActive(true);
    }

    void DoorOpen()
    {
        if (needLV <= ProgressManager.Instance.dataLevel)
        {
            SaveObject();

            OpenDoor();

            UIObjectOn();

            if (!isNotClose)
                Invoke("CloseDoor", openTime);
        }
        else
        {
            if (isTextOn)
            {
                TextOn();
            }
        }

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
        SaveObject();
        UIObjectOn();
        StartCoroutine(MovePlayer(1.5f));
    }

    IEnumerator MovePlayer(float second)
    {
        yield return new WaitForSeconds(second);
        player.transform.position = movePoint.position;
    }

    void ObjectActive()
    {
        SaveObject();
        UIObjectOn();
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
        SaveObject();
        UIObjectOn();
        player.hasWeapons[gunIndex] = true;
        transform.gameObject.SetActive(false);
        ObjectManager.Instance.saveObjects[ObjectID] = false;
    }

    void GetData()
    {
        SaveObject();
        UIObjectOn();
        MaterialManager.Instance.UFSData += DataCount;
        transform.gameObject.SetActive(false);
        ObjectManager.Instance.saveObjects[ObjectID] = false;
        UIManager.Instance.OpenObjectGetText("Get Data");
    }

    void LightOn()
    {
        if (MaterialManager.Instance.UFSData >= needUFSData)
        {
            SaveObject();
            offLight.SetActive(false);
            OnLight.SetActive(true);
            GetComponent<BoxCollider>().enabled = false;
            LightManager.Instance.lightObjects[lightIndex] = true;
            MaterialManager.Instance.UFSData -= needUFSData;
            ProgressManager.Instance.curData += getData;
            ObjectOn();
            UIObjectOn();
        }
        else
        {
            if (isTextOn)
            {
                TextOn();
            }
        }
    }

    void TextOn()
    {
        if (!isText)
        {
            isText = true;

            if(isTrigger)
                UIManager.Instance.InteractionButtonImage(-1);

            SaveObject();

            StartCoroutine(StartTextBox(textBox[curTextCount].duration));

            if (interactionType == Type.TextBox && isOnce)
            {
                GetComponent<BoxCollider>().enabled = false;
            }
            if (isClick)
            {
                player.isCommunicate = true;
                isCommunicate = true;
            }
        }
    }

    IEnumerator StartTextBox(float durationTime)
    {
        UIManager.Instance.textUIAnim.SetTrigger("Open");
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
        UIManager.Instance.textUIAnim.SetTrigger("Close");

        yield return new WaitForSeconds(1f);
        if (curTextCount < textBox.Length - 1)
        {
            curTextCount += 1;

            StartCoroutine(StartTextBox(durationTime));
        }
        else
        {
            curTextCount = 0;
            isText = false;
            if (isClick)
            {
                player.isCommunicate = false;
                isCommunicate = false;
                isNextReady = false;
                if(textEndObjectOn)
                {
                    ObjectOn();
                }
            }
        }
        yield return null;
    }


    void PassWordOn()
    {
        if(!passWord.isDone)
        {
            SaveObject();
            UIObjectOn();

            isActive = true;
            player.isCommunicate = true;
            v_Cam.SetActive(false);
            p_cameraMove.enabled = false;
            StartCoroutine(MoveCameraCoroutine(cameraPos.position, cameraPos.rotation));
        }
    }

    private IEnumerator MoveCameraCoroutine(Vector3 targetPosition, Quaternion targetRotation)
    {
        while (Vector3.Distance(m_Camera.transform.position, targetPosition) > 0.01f || Quaternion.Angle(m_Camera.transform.rotation, targetRotation) > 0.01f)
        {
            // 현재 위치와 목표 위치 사이의 보간
            Vector3 desiredPosition = Vector3.Lerp(m_Camera.transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            Quaternion desiredRotation = Quaternion.Lerp(m_Camera.transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);

            // 부드럽게 이동
            m_Camera.transform.position = desiredPosition;
            m_Camera.transform.rotation = desiredRotation;

            yield return null; // 다음 프레임까지 대기
        }


        passWordUI.SetActive(true);
        

        yield return null;
    }

    void ObjectRotaterOpen()
    {
        player.isCommunicate = true;
        main_Camera.SetActive(false);
        rotate_Camera.SetActive(true);
        rotate_Camera.transform.position = rotate_Camera_Pos.position;
        rotate_Camera.transform.rotation = rotate_Camera_Pos.rotation;

        if (hasUFSData)
        {
            if (data_inst_Object == null)
            {
                data_inst_Object = Instantiate(data_p_Object, ObjectPos.position, data_p_Object.transform.rotation);
                objRotater = data_inst_Object.GetComponent<ObjectRotater>();
                objRotater.player = player;
                objRotater.interaction = GetComponent<Interaction>();
                objRotater.mainCamera = main_Camera;
                objRotater.rotation_Camera = rotate_Camera.GetComponent<Camera>();
            }
            else
            {
                data_inst_Object.SetActive(true);
            }
        }
        else
        {
            if (inst_Object == null)
            {
                inst_Object = Instantiate(p_Object, ObjectPos.position, data_p_Object.transform.rotation);
                objRotater = inst_Object.GetComponent<ObjectRotater>();
                objRotater.player = player;
                objRotater.interaction = GetComponent<Interaction>();
                objRotater.mainCamera = main_Camera;
                objRotater.rotation_Camera = rotate_Camera.GetComponent<Camera>();
            }
            else
            {
                inst_Object.SetActive(true);
            }
        }
    }

    void ObjectOn()
    {
        if (MaterialManager.Instance.UFSData >= o_needUSFData)
        {
            MaterialManager.Instance.UFSData -= o_needUSFData;

            UIObjectOn();

            ObjectOnOff();

            SaveObject();

            transform.gameObject.SetActive(false);
        }
        else
        {
            if (isQuestUpdate)
            {
                UIManager.Instance.questUIAnim.SetTrigger("QuestUpdate");

                if (useUSFDataVariation)
                {
                    UIManager.Instance.questText.text = questText + MaterialManager.Instance.UFSData.ToString() + questTextPlus;
                }
                else
                {
                    UIManager.Instance.questText.text = questText;
                }
            }

            if (isTextOn)
            {
                TextOn();
            }
        }
    }

    void ObjectOnOff()
    {
        for (int i = 0; i < a_col.Length; i++)
            a_col[i].enabled = true;

        for (int i = 0; i < a_gameObj.Length; i++)
            a_gameObj[i].SetActive(true);

        for (int i = 0; i < a_interaction.Length; i++)
            a_interaction[i].enabled = true;

        for (int i = 0; i < d_col.Length; i++)
            d_col[i].enabled = false;

        for (int i = 0; i < d_gameObj.Length; i++)
            d_gameObj[i].SetActive(false);

        for (int i = 0; i < d_interaction.Length; i++)
            d_interaction[i].enabled = false;
    }

    void OpenWeaponBox()
    {
        if (hasOpen)
        {
            TextOn();
        }
        else
        {
            UIObjectOn();

            switch (weaponIndex)
            {
                case 1:
                    MaterialManager.Instance.RifleAmmo += weaponAmount;
                    break;

                case 2:
                    MaterialManager.Instance.ShotgunAmmo += weaponAmount;
                    break;

                case 3:
                    MaterialManager.Instance.LazerAmmo += weaponAmount;
                    break;
            }

            hasOpen = true;
            SaveObject();
        }
    }

    void RotateObject()
    {
        if (!isRotate)
        {
            StartCoroutine(RotateToTargetAngle());
        }
    }

    private IEnumerator RotateToTargetAngle()
    {
        isRotate = true;
        float currentAngle = 0f;

        while (currentAngle < targetAngle)
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;
            if (currentAngle + rotationAmount > targetAngle)
            {
                rotationAmount = targetAngle - currentAngle;
            }

            targetObject.transform.Rotate(0, rotationAmount, 0);
            currentAngle += rotationAmount;

            yield return null;
        }

        isRotate = false;
    }
}

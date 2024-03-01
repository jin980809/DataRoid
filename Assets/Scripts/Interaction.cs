using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
    public string fileName = "Save.csv";
    List<string[]> data = new List<string[]>();
    string[] tempData;

    [Space(10)]
    [Header("MovePlayer")]
    public Transform movePoint;

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

            case Type.SavePoint:
                SaveCSVFile(savePoint);
                break;

            case Type.MovePlayer:
                MovePlayer();
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



    void Awake()
    {
        data.Clear();

        tempData = new string[7];
        tempData[0] = "SavePoint";
        tempData[1] = "Progress";
        tempData[2] = "MaxProgress";
        tempData[3] = "Ammo";
        tempData[4] = "SpecialAmmo";
        tempData[5] = "Steel";
        tempData[6] = "GunPowder";
        data.Add(tempData);
    }


    public void SaveCSVFile(int savePointIndex)
    {
        tempData = new string[7];
        tempData[0] = savePointIndex.ToString();
        tempData[1] = ProgressManager.Instance.curProgress.ToString();
        tempData[2] = ProgressManager.Instance.saveProgress.ToString();
        tempData[3] = MaterialManager.Instance.Ammo.ToString();
        tempData[4] = MaterialManager.Instance.SpecialAmmo.ToString();
        tempData[5] = MaterialManager.Instance.Steel.ToString();
        tempData[6] = MaterialManager.Instance.GunPowder.ToString();
        data.Add(tempData);

        string[][] output = new string[data.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = data[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            sb.AppendLine(string.Join(delimiter, output[i]));
        }

        string filepath = SystemPath.GetPath();

        if (!Directory.Exists(filepath))
        {
            Directory.CreateDirectory(filepath);
        }

        StreamWriter outStream = System.IO.File.CreateText(filepath + fileName);
        outStream.Write(sb);
        outStream.Close();
    }

    void MovePlayer()
    {
        player.transform.position = movePoint.position;
    }
}
